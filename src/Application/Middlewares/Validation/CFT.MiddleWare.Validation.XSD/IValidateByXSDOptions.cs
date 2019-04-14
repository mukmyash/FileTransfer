namespace CFT.MiddleWare.Validation.XSD
{
    public interface IValidateByXSDOptions
    {
        string TargetNamespace { get; set; }
        string XSDPath { get; set; }

        void ValidationParams();
    }
}