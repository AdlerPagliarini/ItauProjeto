using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using ItauProjeto.Models;

namespace ItauProjeto.Funcoes
{
    public class ConfigUpload
    {
        private string Folder { get; set; }
        private string Type { get; set; }
        private int Width { get; set; }
        private int Height { get; set; }
        private string SetReturn { get; set; }
        private string SavePath { get; set; }

        public ConfigUpload()
        {

        }
        public ConfigUpload(string Folder, string Type, int Width, int Height, string SetReturn, string SavePath)
        {
            this.Folder = Folder;
            this.Type = Type;
            this.Width = Width;
            this.Height = Height;
            this.SetReturn = SetReturn;
            this.SavePath = SavePath;
        }
        public string SalvarArquivo(HttpPostedFileBase fileToSave)
        {
            string retorno = salvandoArquivo(fileToSave)[1];
            if (string.IsNullOrEmpty(retorno))
            {
                return retorno;
            }
            else
            {
                return retorno;
            }
        }
        public List<ModelArquivo> SalvarArquivo(IEnumerable<HttpPostedFileBase> files)
        {
            cleanFolderCrop();
            Type = Convert.ToString(Type).ToUpper();

            //aux
            string name = "";
            bool fileCrop = false;
            bool uploadSuccess = false;
            int intArqLargura = 0;
            int intArqAltura = 0;
            string type = ".file";

            List<ModelArquivo> arquivoList = new List<ModelArquivo>();
            foreach (var fileToSave in files)
            {
                if (fileToSave == null) return null;
                if (Type == "IMAGEM")
                {
                    string[] extensoes = { ".png", ".jpg", ".gif", ".jpeg", ".bmp" };
                    foreach (string extensao in extensoes)
                    {
                        if (fileToSave.FileName != "")
                        {

                            if (System.IO.Path.GetExtension(fileToSave.FileName).ToLower() == extensao)
                            {
                                type = extensao;
                                string[] sArqTemp = salvandoArquivo(fileToSave);
                                name = sArqTemp[1];
                                System.Drawing.Image Imgs = System.Drawing.Image.FromFile(sArqTemp[0]);
                                intArqLargura = Convert.ToInt32(Imgs.PhysicalDimension.Width);
                                intArqAltura = Convert.ToInt32(Imgs.PhysicalDimension.Height);

                                if (!liberaUpload(Convert.ToInt32(intArqLargura), Convert.ToInt32(intArqAltura)))
                                {
                                    if (!liberaRedimensionar(Convert.ToInt32(intArqLargura), Convert.ToInt32(intArqAltura)))
                                    {
                                        Imgs.Dispose();
                                        System.IO.File.Delete(sArqTemp[0]);
                                        //retorno = "Arquivo menor que o permitido";

                                        fileCrop = false;
                                        uploadSuccess = false;
                                        break;
                                    }
                                    else
                                    {
                                        Imgs.Dispose();
                                        if (RedimensionarToCrop(sArqTemp[1]))
                                        {
                                            RedimensionarImagem(sArqTemp[1]);
                                            //retorno = "Arquivo Salvo com Sucesso, Sem Crop, Redimensionou 100%";

                                            fileCrop = false;
                                            uploadSuccess = true;

                                            break;
                                        }
                                        else
                                        {
                                            System.IO.File.Delete(sArqTemp[0]);
                                            //retorno = "Arquivo precisa ser cortado";

                                            fileCrop = true;//sArqTemp[1];
                                            uploadSuccess = true;

                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    Imgs.Dispose();
                                    RedimensionarImagem(sArqTemp[1]);
                                    //retorno = "Arquivo Salvo com Sucesso, Sem Crop";

                                    fileCrop = false;
                                    uploadSuccess = true;

                                    break;
                                }
                            }
                        }
                        else
                        {
                            //retorno = "Nenhum arquivo selecionado";
                            fileCrop = false;
                            uploadSuccess = false;

                            break;
                        }
                    }
                }
                else
                {
                    string[] extensoes = { ".png", ".jpg", ".gif", ".jpeg", ".bmp" };
                    bool isImg = false;
                    foreach (string extensao in extensoes)
                    {
                        if (System.IO.Path.GetExtension(fileToSave.FileName).ToLower() == extensao)
                        {
                            type = extensao;
                            isImg = true;
                            break;
                        }
                    }
                    if (!isImg)
                    {
                        string[] sArqTemp = salvandoArquivo(fileToSave);
                        name = sArqTemp[1];
                        //retorno = "Arquivo Salvo com Sucesso [2]";

                        fileCrop = false;
                        uploadSuccess = true;

                    }
                    else
                    {
                        string[] sArqTemp = salvandoArquivo(fileToSave);
                        RedimensionarImagem(sArqTemp[1]);
                        name = sArqTemp[1];
                        //retorno = "Cria Thumb";

                        fileCrop = false;
                        uploadSuccess = true;

                    }
                }


                arquivoList.Add(new ModelArquivo(name, type, Folder, Width, Height, fileCrop, uploadSuccess));
            }
            return arquivoList;
        }
        private string[] salvandoArquivo(HttpPostedFileBase fileToSave)
        {
            string SaveURL = this.SavePath;
            string[] retorno = new string[2];
            try { 
            if (fileToSave.FileName != "")
            {
                SaveURL = this.SavePath + "upload\\" + Folder + "\\" + fileToSave.FileName;

                if (File.Exists(SaveURL))
                {
                    int rename = 1;
                    SaveURL = this.SavePath + "upload\\" + Folder + "\\";
                    string[] arqRename = fileToSave.FileName.ToString().Split('.');
                    while (File.Exists(SaveURL + arqRename[0] + "_" + rename + "." + arqRename[1]))
                    {
                        rename = rename + 1;
                    }
                    fileToSave.SaveAs(SaveURL + arqRename[0] + "_" + rename + "." + arqRename[1]);
                    retorno[0] = SaveURL + arqRename[0] + "_" + rename + "." + arqRename[1];
                    retorno[1] = arqRename[0] + "_" + rename + "." + arqRename[1];
                    //lblMsg.Text = "Arquivo Salvo com Sucesso";
                    return retorno;
                }
                else
                {
                    fileToSave.SaveAs(SaveURL);
                    retorno[0] = SaveURL;
                    retorno[1] = fileToSave.FileName;
                    //lblMsg.Text = "Arquivo Salvo com Sucesso";
                    return retorno;
                }
            }
            else
            {
                //lblMsg.Text = "Nenhum arquivo selecionado";
                return retorno;
            }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public bool liberaUpload(int w, int h)
        {
            if (Type.ToUpper() == "IMAGEM")
            {
                if (Width == 0 && Height == 0)
                {
                    return true;
                }
                else if (Width == 0 && Height == h)
                {
                    return true;
                }
                else if (Width == w && Height == 0)
                {
                    return true;
                }
                else if (Width == w && Height == h)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }
        private bool liberaRedimensionar(int w, int h)
        {
            if (Width > w || Height > h)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private void RedimensionarImagem(string fileNameSave)
        {
            string arquivo;
            string[] extensoes = { ".png", ".jpg", ".gif", ".jpeg", ".bmp" };
            foreach (string extensao in extensoes)
            {
                if (System.IO.Path.GetExtension(fileNameSave).ToLower() == extensao)
                {
                    try
                    {
                        arquivo = fileNameSave;//fileImg.FileName;

                        arquivo = SavePath + "upload\\" + Folder + "\\" + arquivo;

                        System.Drawing.Image fullSizeImg = System.Drawing.Image.FromFile(arquivo);
                        int originalwidth = fullSizeImg.Width;
                        int originalheight = fullSizeImg.Height;

                        System.Drawing.Image thumbNailImg = EscalaPercentual(fullSizeImg, 100, 0, originalwidth, originalheight);

                        String nome = fileNameSave;

                        thumbNailImg.Save(SavePath + "upload\\" + Folder + "\\min\\" + nome, myImgFormat(System.IO.Path.GetExtension(fileNameSave).ToLower()));

                        fullSizeImg.Dispose();
                        thumbNailImg.Dispose();

                    }
                    catch (Exception ex)
                    {
                        //Error;
                    }
                    break;
                }
            }
        }

        private bool RedimensionarToCrop(string fileNameSave)
        {
            string[] extensoes = { ".png", ".jpg", ".gif", ".jpeg", ".bmp" };
            string arquivo;
            foreach (string extensao in extensoes)
            {
                if (System.IO.Path.GetExtension(fileNameSave).ToLower() == extensao)
                {
                    try
                    {
                        arquivo = fileNameSave;//fileImg.FileName;

                        arquivo = SavePath + "upload\\" + Folder + "\\" + arquivo;

                        System.Drawing.Image fullSizeImg = System.Drawing.Image.FromFile(arquivo);
                        int originalwidth = fullSizeImg.Width;
                        int originalheight = fullSizeImg.Height;
                        int newHeight = (originalheight * Width) / originalwidth;
                        int newWidth = (originalwidth * Height) / originalheight;

                        System.Drawing.Image thumbNailImg;
                        String nome = fileNameSave;
                        if ((newWidth == Width) && (newHeight == Height))
                        {
                            thumbNailImg = EscalaPercentual(fullSizeImg, Height, 0, originalwidth, originalheight);
                            fullSizeImg.Dispose();
                            System.IO.File.Delete(nome);
                            thumbNailImg.Save(SavePath + "upload\\" + Folder + "\\" + nome, myImgFormat(System.IO.Path.GetExtension(fileNameSave).ToLower()));
                            thumbNailImg.Dispose();
                            //100% com sucesso, não precisa recorta
                            return true;
                        }
                        else if (newHeight < Height)
                        {
                            thumbNailImg = EscalaPercentual(fullSizeImg, Height, 0, originalwidth, originalheight);
                            fullSizeImg.Dispose();
                            thumbNailImg.Save(SavePath + "upload\\" + Folder + "\\crop\\" + nome, myImgFormat(System.IO.Path.GetExtension(fileNameSave).ToLower()));
                            thumbNailImg.Dispose();
                            //Precisa cortar a imagem
                            return false;
                        }
                        else //if (newWidth < width)
                        {
                            thumbNailImg = EscalaPercentual(fullSizeImg, 0, Width, originalwidth, originalheight);
                            thumbNailImg.Save(SavePath + "upload\\" + Folder + "\\crop\\" + nome, myImgFormat(System.IO.Path.GetExtension(fileNameSave).ToLower()));
                            fullSizeImg.Dispose();
                            thumbNailImg.Dispose();
                            //Precisa cortar a imagem
                            return false;
                        }
                    }//tryy

                    catch (Exception ex)
                    {
                        //Error;
                        return false;
                    }
                }
            }
            return false;
        }

        static System.Drawing.Image EscalaPercentual(System.Drawing.Image imgPhoto, int newHeight, int newWidth, int width, int height)
        {
            int fonteLargura = width;     //armazena a largura original da imagem origem
            int fonteAltura = height;   //armazena a altura original da imagem origem
            int origemX = 0;        //eixo x da imagem origem
            int origemY = 0;        //eixo y da imagem origem

            int destX = 0;          //eixo x da imagem destino
            int destY = 0;          //eixo y da imagem destino
            //Calcula a altura e largura da imagem redimensionada
            int destWidth = 0;
            int destHeight = 0;
            if (newWidth != 0)
            {
                destWidth = (int)newWidth;
                destHeight = (int)(newWidth * fonteAltura) / fonteLargura;
            }
            else //if(newHeight != 0)
            {
                destWidth = (int)(newHeight * fonteLargura) / fonteAltura;
                destHeight = (int)(newHeight);
            }

            //Cria um novo objeto bitmap
            Bitmap bmImagem = new Bitmap(destWidth, destHeight, PixelFormat.Format32bppArgb);
            bmImagem.MakeTransparent(Color.White);     // Change a color to be transparent
            //Define a resolução do bitmap.
            bmImagem.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);
            //Crima um objeto graphics e defina a qualidade
            Graphics grImagem = Graphics.FromImage(bmImagem);
            grImagem.InterpolationMode = InterpolationMode.HighQualityBicubic;

            //Desenha a imge usando o método DrawImage() da classe grafica
            grImagem.DrawImage(imgPhoto,
                new Rectangle(destX - 1, destY - 1, destWidth + 1, destHeight + 1),
                new Rectangle(origemX, origemY, fonteLargura, fonteAltura), // coloquei +1,+1,-1,-1 para ele perde 1 pixel porem, n corte um pixel preto
                GraphicsUnit.Pixel);

            grImagem.Dispose();  //libera o objeto grafico
            return bmImagem;
        }

        private ImageFormat myImgFormat(string value)
        {
            if (value == ".png") { return ImageFormat.Png; }
            else if (value == ".jpg" || value == ".jpeg") { return ImageFormat.Jpeg; }
            else if (value == ".gif") { return ImageFormat.Gif; }
            else if (value == ".bmp") { return ImageFormat.Bmp; }
            else//default
            {
                return ImageFormat.Png;
            }
        }
        private void cleanFolderCrop()
        {
            try
            {
                DirectoryInfo pasta;
                pasta = new DirectoryInfo(SavePath + "upload\\" + Folder + "\\crop");
                FileInfo[] arquivos = pasta.GetFiles().OrderByDescending(p => p.CreationTime).ToArray();
                System.Collections.Generic.List<string> oList = new System.Collections.Generic.List<string>();
                foreach (FileInfo file in arquivos)
                {
                    System.IO.File.Delete(SavePath + "upload\\" + Folder + "\\crop\\" + file.Name);
                }
            }
            catch (Exception ex)
            {
                //Error;
            }
        }


        public bool CortarArquivo(string fileName, int X, int Y, int W, int H)
        {
            // Crop Image Here & Save
            string filePath = SavePath + "upload\\" + Folder + "\\crop\\" + fileName;
            string cropFileName = "";
            string cropFilePath = "";
            if (File.Exists(filePath))
            {
                System.Drawing.Image orgImg = System.Drawing.Image.FromFile(filePath);
                Rectangle CropArea = new Rectangle(
                    Convert.ToInt32(X),
                    Convert.ToInt32(Y),
                    Convert.ToInt32(W),
                    Convert.ToInt32(H));
                try
                {
                    Bitmap bitMap = new Bitmap(CropArea.Width, CropArea.Height);
                    using (Graphics g = Graphics.FromImage(bitMap))
                    {
                        g.DrawImage(orgImg, new Rectangle(0, 0, bitMap.Width, bitMap.Height), CropArea, GraphicsUnit.Pixel);
                    }
                    cropFileName = fileName;
                    cropFilePath = SavePath + "upload\\" + Folder + "\\";
                    bitMap.Save(cropFilePath + fileName, myImgFormat(System.IO.Path.GetExtension(fileName).ToLower()));
                    RedimensionarImagem(fileName);
                }
                catch (Exception ex)
                {
                    return false;
                }
                orgImg.Dispose();
            }
            return true;
        }

        public List<ModelArquivo> carregarArquivos()
        {
            List<ModelArquivo> arquivoList = new List<ModelArquivo>();
            DirectoryInfo pasta;
            pasta = new DirectoryInfo(SavePath + "upload\\" + Folder);
            FileInfo[] arquivos = pasta.GetFiles().OrderByDescending(p => p.CreationTime).ToArray();

            string TypeAux = "";
            bool AllowSelectAux = false;
            int originalwidth = 0;
            int originalheight = 0;

            foreach (FileInfo file in arquivos)
            {
                string extensao = System.IO.Path.GetExtension(file.Name).ToLower();
                string tipo = Convert.ToString(Type).ToUpper();


                //verifica se eh imagem e coloca tamanho e ve se a imagem eh do tamanho certo
                if (extensao == ".png" || extensao == ".jpg" || extensao == ".gif" || extensao == ".jpeg" || extensao == ".bmp")
                {
                    TypeAux = extensao;

                    string imagem = SavePath + "upload\\" + Folder + "\\" + file.Name;
                    System.Drawing.Image fullSizeImg = System.Drawing.Image.FromFile(imagem);
                    originalwidth = fullSizeImg.Width;
                    originalheight = fullSizeImg.Height;
                    if (liberaUpload(originalwidth, originalheight))
                    {
                        AllowSelectAux = true;
                    }
                    else
                    {
                        AllowSelectAux = false;
                    }
                    fullSizeImg.Dispose();
                }
                else if (tipo == "ARQUIVO")
                {
                    AllowSelectAux = true;
                    TypeAux = ".file";
                }
                else
                {
                    AllowSelectAux = true;
                    TypeAux = ".file";
                }

                arquivoList.Add(new ModelArquivo(file.Name, TypeAux, Folder, originalwidth, originalheight, false, false, AllowSelectAux));
            }
            return arquivoList;
        }

        public void ExcluirArquivo(string file)
        {
            if (File.Exists(SavePath + "upload\\" + Folder + "\\Min\\" + file))
                File.Delete(SavePath + "upload\\" + Folder + "\\Min\\" + file); //Deleta a imagem
            if (File.Exists(SavePath + "upload\\" + Folder + "\\" + file))
                File.Delete(SavePath + "upload\\" + Folder + "\\" + file);
        }
    }
}