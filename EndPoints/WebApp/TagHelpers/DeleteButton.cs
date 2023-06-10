using Microsoft.AspNetCore.Razor.TagHelpers;

namespace WebApp.TagHelpers
{
    [HtmlTargetElement("DeleteItem", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class DeleteButton : TagHelper
    {
        public string Url { get; set; }
        public string Class { get; set; } = "btn btn-sm btn-danger ";
        public string SuccessMessage { get; set; } = null;
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
           
            output.TagName = "button";
            output.Attributes.Add("class", Class);
            output.Attributes.Add("type", "button");
            output.Attributes.Add("onclick", $"deleteItem('{Url}','{SuccessMessage}')");
        }
    }
}