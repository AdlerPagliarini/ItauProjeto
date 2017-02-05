using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ItauProjeto.Models
{
    public class ModelArquivo
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Path { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool FileCrop { get; set; }
        public bool AllowUpload { get; set; }
        public bool AllowSelect { get; set; }

        public ModelArquivo()
        {

        }
        //Arquivo Upload, usado no configUpload
        public ModelArquivo(string Name, string Type, string Path, int Width, int Height, bool FileCrop, bool AllowUpload)
        {
            this.Name = Name;
            this.Type = Type;
            this.Path = Path;
            this.Width = Width;
            this.Height = Height;
            this.FileCrop = FileCrop;
            this.AllowUpload = AllowUpload;
        }
        //Arquivo Lista, para listar os arquivos
        public ModelArquivo(string Name, string Type, string Path, int Width, int Height, bool FileCrop, bool AllowUpload, bool AllowSelect)
        {
            this.Name = Name;
            this.Type = Type;
            this.Path = Path;
            this.Width = Width;
            this.Height = Height;
            this.FileCrop = FileCrop;
            this.AllowUpload = AllowUpload;
            this.AllowSelect = AllowSelect;
        }


    }
}