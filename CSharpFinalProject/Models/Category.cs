namespace CSharpFinalProject.Models
{
    internal class Category
    {
        public required string Name { get; set; }
        public List<Product>? Products { get; set; }
    }
}
