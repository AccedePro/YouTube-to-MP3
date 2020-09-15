using MediaToolkit;
using MediaToolkit.Model;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using VideoLibrary;

namespace YouTube_to_MP3
{
    public partial class frmYT2MP3 : Form
    {
        public frmYT2MP3()
        {
            InitializeComponent();
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            btnConvert.Enabled = false;

            String strPathFromDialog;

            if (rbMP3.Checked == true)
            {
                try
                {
                    var youtube = YouTube.Default;
                    var vid = youtube.GetVideo(txtboxEnterURL.Text);
                    sfdDownload.FileName = vid.Title;
                } catch (Exception)
                {
                    MessageBox.Show("Something went wrong, likely because the video URL is invalid or has been left blank. Please try using an alternate video URL.");
                    btnConvert.Enabled = true;
                    return;
                }

                sfdDownload.Filter = "MPEG Audio Layer-3 |*.mp3 | All FIles |*.*";

            }
            else if (rbMP4.Checked == true)
            {
                try
                {
                    var youtube = YouTube.Default;
                    var vid = youtube.GetVideo(txtboxEnterURL.Text);
                    sfdDownload.FileName = vid.Title;
                }
                catch (Exception)
                {
                    MessageBox.Show("Something went wrong, likely because the video URL is invalid or has been left blank. Please try using an alternate video URL.");
                    btnConvert.Enabled = true;
                    return;
                }

                sfdDownload.Filter = "MPEG 4 PART 14 |*.mp4 | All FIles |*.*";

            }
            else
            {
                MessageBox.Show("You have not selected a file format.");
                btnConvert.Enabled = true;
                return;
            }

            if (sfdDownload.ShowDialog() == DialogResult.OK)
            {

                strPathFromDialog = sfdDownload.FileName;

                if (txtboxEnterURL.Text != "")
                {
                    try
                    {
                        var source = @strPathFromDialog;
                        var youtube = YouTube.Default;
                        var vid = youtube.GetVideo(txtboxEnterURL.Text);
                        pbConvert.Value = 5;
                        System.Threading.Thread.Sleep(500); 

                        File.WriteAllBytes(source, vid.GetBytes());
                        pbConvert.Value = 25; 
                        System.Threading.Thread.Sleep(500);

                        pbConvert.Value = 30;
                        System.Threading.Thread.Sleep(500);

                        var inputFile = new MediaFile { Filename = source };
                        var outputFile = new MediaFile { Filename = $"{source}.mp3" };
                        pbConvert.Value = 75;
                        System.Threading.Thread.Sleep(500);
                        using (var engine = new Engine())
                        {
                            engine.GetMetadata(inputFile);
                            engine.Convert(inputFile, outputFile);
                        }
                        pbConvert.Value = 100;
                        System.Threading.Thread.Sleep(500);
                        MessageBox.Show("Conversion complete.");
                        txtboxEnterURL.Text = "";
                        pbConvert.Value = 0;

                        if (rbMP3.Checked)
                        {

                            File.Delete(source);
                            System.IO.File.Move(source + ".mp3", source);

                        }
                        else if (rbMP4.Checked)
                        {

                            File.Delete(source + ".mp3");

                        }
                        else
                        {
                            return;
                        }
                        btnConvert.Enabled = true;
                    } catch (Exception)
                    {
                        MessageBox.Show("Something went wrong, likely because the video URL is invalid. Please try using an alternate video URL.");
                        pbConvert.Value = 0;
                        txtboxEnterURL.Text = "";
                        btnConvert.Enabled = true;
                        return; 
                    }

                }
                else
                {
                    return;
                }
            }
            else
            {
                btnConvert.Enabled = true;
                return;
            }
        }

        private void frmYT2MP3_Load(object sender, EventArgs e)
        {
            pbConvert.Value = 0;
        }

    }
}

