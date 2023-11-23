namespace FormsAPI.Models
{
    public class Component
    {
        public int id { get; set; }
        public int typeComponentId { get; set; }
        public TypeComponent? typeComponent { get; set; }
        public string text { get; set; }
        public string value { get; set; }
        public bool isVisible { get; set; }
        public bool isEnable { get; set; }
        public string placeHolder { get; set; }
        public string nameDescription { get; set; }
        public int formId { get; set; }
        public Form? form { get; set; }
        public string componentNameId { get; set; }
        
    }
}
