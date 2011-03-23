using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using WorkflowServices.Service;
using WorkflowServices.DataService;
using WorkflowServices.Service.Activities;

namespace WorkflowServices
{
    public enum ModifyProductDataState
    {
        SERVICE_ERROR,
        DATA_ERROR,
        EXIST,
        NOT_EXIST,
        NOT_EXECUTED,
        EXECUTED
    }

    public sealed class ModifyProduct : CodeActivity
    {
        // Définir un argument d'entrée d'activité de type string
        public InArgument<String> Nom { get; set; }
        public InArgument<String> Marque { get; set; }
        public InArgument<String> Reference { get; set; }
        public InArgument<Int32> Stock { get; set; }
        public InArgument<Decimal> Prix { get; set; }
        public InArgument<String> Description { get; set; }
        public InArgument<Guid> ProductGuid { get; set; }
        public InArgument<Guid> UserGuid { get; set; }
        public InArgument<UserType> Type { get; set; }
        public InArgument<Boolean> Disponible { get; set; }
        public InArgument<Guid[]> Categorie { get; set; }
        public OutArgument<ModifyProductDataState> State { get; set; }

        // Si votre activité retourne une valeur, dérivez de CodeActivity<TResult>
        // et retournez la valeur à partir de la méthode Execute.
        protected override void Execute(CodeActivityContext context)
        {
            string nom = context.GetValue<String>(this.Nom);
            string marque = context.GetValue<String>(this.Marque);
            string reference = context.GetValue<String>(this.Reference);
            string description = context.GetValue<String>(this.Description);
            int stock = context.GetValue<Int32>(this.Stock);
            decimal prix = context.GetValue<Decimal>(this.Prix);
            bool disponible = context.GetValue<Boolean>(this.Disponible);
            Guid userGuid = context.GetValue<Guid>(this.UserGuid);
            Guid productGuid = context.GetValue<Guid>(this.ProductGuid);
            UserType type = context.GetValue<UserType>(this.Type);
            Guid[] categorie = context.GetValue<Guid[]>(this.Categorie);
            ModifyProductDataState state = ModifyProductDataState.NOT_EXECUTED;
            try
            {
                var ctx = new DADEntities(new Uri(Properties.Settings.Default.DataService));
                ctx.IgnoreResourceNotFoundException = true;

                var product = (from p in ctx.PRODUIT.Expand("FOURNISSEUR,CATEGORIE")
                               where p.id == productGuid
                               select p).FirstOrDefault<PRODUIT>();

                var qry = from c in ctx.CATEGORIE                                  
                                  select c;

                List<CATEGORIE> allCategories = qry.ToList<CATEGORIE>();

                var categories = from CATEGORIE c in allCategories
                                 where categorie.Contains<Guid>(c.id)
                                 select c;

                if (product != null)
                {
                    state = ModifyProductDataState.EXIST;
                    ChangeItemAvailability activity1 = new ChangeItemAvailability();
                    AdminProductModification activity2 = new AdminProductModification();
                    Dictionary<string, object> arguments1 = new Dictionary<string, object>();
                    Dictionary<string, object> arguments2 = new Dictionary<string, object>();
                    if (type == UserType.ADMINISTRATOR)
                    {
                        product.disponible = disponible;
                        arguments2.Add("itemID", product.id);
                        arguments2.Add("fournisseurID", product.FOURNISSEUR.id);
                        arguments2.Add("deleted", false);
                    }
                    else if (type == UserType.FOURNISSEUR && userGuid == product.FOURNISSEUR.id)
                    {
                        product.nom = nom;
                        product.reference = reference;
                        product.marque = marque;
                        product.description = description;
                        product.stock = stock;
                        product.prix = prix;
                        product.disponible = disponible;
                    }
                    arguments1.Add("available", product.disponible);
                    arguments1.Add("exist", product.supprime);
                    arguments1.Add("itemID", product.id);
                    arguments1.Add("quantity", product.stock);

                    foreach (CATEGORIE cat in categories)
                    {
                        if (!product.CATEGORIE.Contains(cat))
                        {
                            product.CATEGORIE.Add(cat);
                            ctx.AddLink(product, "CATEGORIE", cat);
                            cat.PRODUIT.Add(product);
                            ctx.AddLink(cat, "PRODUIT", product);
                        }
                    }

                    List<CATEGORIE> catToDelete = new List<CATEGORIE>();

                    foreach (CATEGORIE cat in product.CATEGORIE)
                    {
                        if (!categories.Contains(cat))
                        {
                            catToDelete.Add(cat);
                            ctx.DeleteLink(product, "CATEGORIE", cat);
                            cat.PRODUIT.Remove(product);
                            ctx.DeleteLink(cat, "PRODUIT", product);
                        }
                    }

                    foreach (CATEGORIE cat in catToDelete)
                    {
                        product.CATEGORIE.Remove(cat);
                    }

                    ctx.UpdateObject(product);
                    ctx.SaveChanges(System.Data.Services.Client.SaveChangesOptions.Batch);

                    state = ModifyProductDataState.EXECUTED;

                    WorkflowInvoker.Invoke(activity1, arguments1);

                    if(type == UserType.ADMINISTRATOR)
                    {
                        WorkflowInvoker.Invoke(activity2, arguments2);
                    }
                }
                else
                {
                    state = ModifyProductDataState.NOT_EXIST;
                }
            }
            catch
            {
                if (state != ModifyProductDataState.EXECUTED)
                    state = ModifyProductDataState.DATA_ERROR;
                else
                    state = ModifyProductDataState.SERVICE_ERROR;
            }
            context.SetValue<ModifyProductDataState>(this.State, state);
        }
    }
}
