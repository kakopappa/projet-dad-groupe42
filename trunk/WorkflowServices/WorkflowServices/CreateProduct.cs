using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using WorkflowServices.DataService;

namespace WorkflowServices
{
    public enum CreateProductState
    {
        SERVICE_ERROR,
        DATA_ERROR,
        NOT_EXECUTED,
        EXECUTED
    }

    public sealed class CreateProduct : CodeActivity
    {
        public InArgument<String> Nom { get; set; }
        public InArgument<String> Marque { get; set; }
        public InArgument<String> Reference { get; set; }
        public InArgument<Int32> Stock { get; set; }
        public InArgument<Decimal> Prix { get; set; }
        public InArgument<String> Description { get; set; }
        public InArgument<Guid> UserGuid { get; set; }
        public InArgument<Boolean> Disponible { get; set; }
        public InArgument<Guid[]> Categorie { get; set; }
        public InArgument<String> Url { get; set; }
        public OutArgument<CreateProductState> State { get; set; }

        // Si votre activité retourne une valeur, dérivez de CodeActivity<TResult>
        // et retournez la valeur à partir de la méthode Execute.
        protected override void Execute(CodeActivityContext context)
        {
            string nom = context.GetValue<String>(this.Nom);
            string marque = context.GetValue<String>(this.Marque);
            string reference = context.GetValue<String>(this.Reference);
            string description = context.GetValue<String>(this.Description);
            string url = context.GetValue<String>(this.Url);
            int stock = context.GetValue<Int32>(this.Stock);
            decimal prix = context.GetValue<Decimal>(this.Prix);
            bool disponible = context.GetValue<Boolean>(this.Disponible);
            Guid userGuid = context.GetValue<Guid>(this.UserGuid);
            Guid[] categorie = context.GetValue<Guid[]>(this.Categorie);
            CreateProductState state = CreateProductState.NOT_EXECUTED;
            try
            {
                var ctx = new DADEntities(new Uri(Properties.Settings.Default.DataService));
                ctx.IgnoreResourceNotFoundException = true;

                var fournisseur = (from f in ctx.FOURNISSEUR.Expand("PRODUIT")
                                   where f.id == userGuid
                                   select f).FirstOrDefault<FOURNISSEUR>();

                var qry = from c in ctx.CATEGORIE
                          select c;

                List<CATEGORIE> allCategories = qry.ToList<CATEGORIE>();

                var categories = from CATEGORIE c in allCategories
                                 where categorie.Contains<Guid>(c.id)
                                 select c;

                PRODUIT product = PRODUIT.CreatePRODUIT(Guid.NewGuid(), reference, nom, marque, prix, stock, disponible, false);

                product.description = description;
                ctx.AddRelatedObject(fournisseur, "PRODUIT", product);
                product.FOURNISSEUR = fournisseur;
                fournisseur.PRODUIT.Add(product);


                foreach (CATEGORIE cat in categories)
                {
                    product.CATEGORIE.Add(cat);
                    ctx.AddLink(product, "CATEGORIE", cat);
                    cat.PRODUIT.Add(product);
                    ctx.AddLink(cat, "PRODUIT", product);
                }
                
                ctx.SaveChanges(System.Data.Services.Client.SaveChangesOptions.Batch);

                if (!string.IsNullOrWhiteSpace(url))
                {
                    IMAGE image = IMAGE.CreateIMAGE(Guid.NewGuid(), url, 0, 0);
                    image.PRODUIT = product;
                    ctx.AddRelatedObject(product, "IMAGE", image);
                    product.IMAGE.Add(image);
                    ctx.SetLink(image, "PRODUIT", product);
                }

                ctx.SaveChanges(System.Data.Services.Client.SaveChangesOptions.Batch);
                state = CreateProductState.EXECUTED;
            }
            catch
            {
                state = CreateProductState.DATA_ERROR;
            }
            context.SetValue<CreateProductState>(this.State, state);
        }
    }
}
