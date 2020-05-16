using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Nero.ResourceManager
{
    public partial class MainForm : Form
    {
        string[] cacheFiles = { };

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var open = new OpenFileDialog();
            open.Filter = "All files (*.*)|*.*";
            open.RestoreDirectory = true;
            open.Multiselect = true;

            if (open.ShowDialog() == DialogResult.OK)
            {
                var files = open.FileNames;
                if (files.Length > 0)
                {
                    cacheFiles = files;
                    lFiles.Text = $"{files.Length} arquivos abertos";
                }
            }
        }

        private void salvarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cacheFiles.Length == 0)
            {
                MessageBox.Show("Não há um arquivo aberto!");
                return;
            }

            if (cacheFiles.Length > 0)
                foreach (var f in cacheFiles)
                {
                    byte[] data = { };
                    using (var r = File.OpenRead(f))
                    {
                        data = new byte[r.Length];
                        r.Read(data, 0, (int)r.Length);
                    }

                    // Comprimir
                    data = MemoryService.Compress(data);

                    // Salvar
                    var ext = f.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
                    using (var fs = File.Create(f.Substring(0, f.Length - ext.Length) + "res"))
                    using (var w = new BinaryWriter(fs))
                    {
                        w.Write(ext);
                        w.Write(data.Length);
                        w.Write(data);
                    }
                }


            MessageBox.Show(cacheFiles.Length > 1 ? "Os arquivos foram comprimidos com sucesso!" : "O arquivo foi comprimido com sucesso!");

        }

        private void decryptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cacheFiles.Length == 0)
            {
                MessageBox.Show("Não há um arquivo aberto!");
                return;
            }

            if (cacheFiles.Length > 0)
                foreach (var f in cacheFiles)
                {
                    if (File.Exists(f))
                    {
                        byte[] data = { };
                        string originalext = "";
                        using (var m = File.OpenRead(f))
                        using (var r = new BinaryReader(m))
                        {
                            originalext = r.ReadString();
                            var l = r.ReadInt32();
                            data = r.ReadBytes(l);
                        }

                        // Descomprimir
                        data = MemoryService.Decompress(data);

                        // Salvar
                        var ext = f.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
                        using (var fs = File.Create(f.Substring(0, f.Length - ext.Length) + originalext))
                        using (var w = new BinaryWriter(fs))
                        {  
                            w.Write(data);
                        }
                    }
                }


            MessageBox.Show(cacheFiles.Length > 1 ? "Os arquivos foram descomprimidos com sucesso!" : "O arquivo foi descomprimido com sucesso!");
        }
    }
}
