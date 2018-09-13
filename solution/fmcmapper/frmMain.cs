using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using log4net;

namespace fmcmapper
{
    public partial class frmMain : Form
    {
        int _imgWidth = 0;
        int _imgHeight = 0;

        int _pixelsPerRow = 20; //may be configurable in future..
        int _pixelsPerCol = 20;

        int _numCols = 0;
        int _numRows = 0;

        short[] _map = null;

        //stores the bt sensor id against rowXcol of sensor
        Dictionary<string, string> _bt_sensors = new Dictionary<string, string>();

        //to be adapted later to project folder
        string _bgImagePath = string.Empty;

        //whenever we make any change
        bool _dirty = false;//set to true when user makes any change and project requires saving


        enum Tiles {Obstacle=100, Bluetooth=200};
        SolidBrush obstacleBrush = null;
        SolidBrush bluetoothBrush = null;

        Bitmap gridImage = null;
        Bitmap obstaclesImage = null;
        Bitmap bluetoothImage = null;
        Font fntText = null;

        protected ILog _log = LogManager.GetLogger("frmMain");

        public frmMain()
        {
            InitializeComponent();

            try
            {
                obstacleBrush = new SolidBrush(Color.FromArgb(50, 255, 0, 0));
                bluetoothBrush = new SolidBrush(Color.FromArgb(50, 0, 0, 255));
                fntText = new Font("Arial", 10);

            }catch(Exception ex)
            {
                _log.Error(ex);
            }
        }

        private void createGridImage()
        {
            gridImage = new Bitmap(_imgWidth, _imgHeight);
            Graphics g = Graphics.FromImage(gridImage);

            for (int i = 0; i < _numRows; i++)
            {
                g.DrawLine(Pens.Black,
                    0, i * _pixelsPerRow, _imgWidth, i * _pixelsPerRow);

                g.DrawString("" + i, SystemFonts.DefaultFont, Brushes.Black, 3, i * _pixelsPerRow);
            }

            for (int j = 0; j < _numCols; j++)
            {
                g.DrawLine(Pens.Black,
                    j * _pixelsPerCol, 0, j * _pixelsPerCol, _imgHeight);

                g.DrawString("" + j, SystemFonts.DefaultFont, Brushes.Black, j * _pixelsPerCol, 3);
            }
        }

        private void createmapImage()
        {
            obstaclesImage = new Bitmap(_imgWidth, _imgHeight);
        }

        private void createBluetoothImage()
        {
            bluetoothImage = new Bitmap(_imgWidth, _imgHeight);
        }

        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            if(
                pictureBox2.BackgroundImage is null
                ||
                gridImage is null
                ||
                obstaclesImage is null
                ||
                bluetoothImage is null
            )
            {
                return;
            }

            e.Graphics.DrawImage(gridImage, 0, 0, gridImage.Width, gridImage.Height);
            e.Graphics.DrawImage(obstaclesImage, 0, 0, _imgWidth, _imgHeight);
            e.Graphics.DrawImage(bluetoothImage, 0, 0, _imgWidth, _imgHeight);

        }

        private void pictureBox2_Click(object sender, EventArgs oe)
        {
            var e = oe as MouseEventArgs;
            if (e == null)
            {
                return;
            }

            if (chkObstacle.Checked)
            {
                mark(e, Tiles.Obstacle);
            }
            else if(chkBluetooth.Checked)
            {
                mark(e, Tiles.Bluetooth);
            }
            else if (chkClear.Checked)
            {
                clearObstacle(e);
            }

        }

        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {

            if (_dirty)
            {
                this.Text = "Find My Car Mapper [Unsaved Changes]";
            }
            else
            {
                this.Text = "Find My Car Mapper";
            }

            if (chkObstacle.Checked)
            {
                mark(e, Tiles.Obstacle);
            }
            else if (chkBluetooth.Checked)
            {
                mark(e, Tiles.Bluetooth);
            }
            else if (chkClear.Checked)
            {
                clearObstacle(e);
            }
        }

        private void clearObstacle(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            _dirty = true;

            int clickedRow = (e.Y - (e.Y % _pixelsPerRow)) / _pixelsPerRow;
            int clickedColumn = (e.X - (e.X % _pixelsPerCol)) / _pixelsPerCol;

            if (clickedRow > _numRows || clickedColumn > _numCols)
            {
                return;
            }

            Bitmap img = obstaclesImage;
            switch(_map[clickedRow * _numCols + clickedColumn])
            {
                case 0:
                    {
                        return;
                    }
                    
                case (short)Tiles.Obstacle:
                    {
                        img = obstaclesImage;
                    }
                    break;
                case (short)Tiles.Bluetooth:
                    {
                        img = bluetoothImage;
                        string index = string.Format("{0}:{1}", clickedRow, clickedColumn);
                        _bt_sensors.Remove(index);
                    }
                    break;
            }

            //fill with rect
            _map[clickedRow * _numCols + clickedColumn] = 0;
            Graphics g = Graphics.FromImage(img);

            GraphicsPath eraseArea = new GraphicsPath();
            eraseArea.AddRectangle(new Rectangle(clickedColumn * _pixelsPerCol, clickedRow * _pixelsPerRow, _pixelsPerCol+1, _pixelsPerRow+1));
            g.SetClip(eraseArea);
            g.Clear(Color.Transparent);
            g.ResetClip();
            
            this.pictureBox2.Refresh();
        }

        private void mark(MouseEventArgs e, Tiles tile)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            _dirty = true;

            int clickedRow = (e.Y - (e.Y % _pixelsPerRow)) / _pixelsPerRow;
            int clickedColumn = (e.X - (e.X % _pixelsPerCol)) / _pixelsPerCol;
            
            if (clickedRow > _numRows || clickedColumn > _numCols)
            {
                return;
            }

            if(_map[clickedRow * _numCols + clickedColumn] != 0)
            {
                return;
            }

            //defaults
            Pen pen = Pens.Red;
            Brush brush = obstacleBrush;
            string textToDraw = string.Empty;
            Bitmap img = obstaclesImage;

            //bluetooth specific
            if(tile == Tiles.Bluetooth)
            {
                FrmBluetoothSensorId btID = new FrmBluetoothSensorId();
                if( btID.ShowDialog() == DialogResult.Cancel)
                {
                    return;
                }

                string index = string.Format("{0}:{1}", clickedRow, clickedColumn);
                _bt_sensors[index] = btID.BluetoothAddress;
                textToDraw = btID.BluetoothAddress;

                pen = Pens.CornflowerBlue;
                brush = bluetoothBrush;
                img = bluetoothImage;

            }

            //fill with rect
            _map[clickedRow * _numCols + clickedColumn] = (short)tile;
            Graphics g = Graphics.FromImage(img);

            g.DrawRectangle(pen, clickedColumn * _pixelsPerCol, clickedRow * _pixelsPerRow, _pixelsPerCol, _pixelsPerRow);
            g.FillRectangle(brush, clickedColumn * _pixelsPerCol, clickedRow * _pixelsPerRow, _pixelsPerCol, _pixelsPerRow);

            if (!string.IsNullOrEmpty(textToDraw))
            {
                g.DrawString(textToDraw,
                    fntText, 
                    Brushes.Black,
                    clickedColumn * _pixelsPerCol + _pixelsPerCol,
                    clickedRow * _pixelsPerRow + _pixelsPerRow, 
                    StringFormat.GenericDefault);
            }

            this.pictureBox2.Refresh();
        }

        private void reset()
        {
            //check dirty
            if (_dirty)
            {
                switch (MessageBox.Show("Save changes?", "Unsaved changes", MessageBoxButtons.YesNoCancel))
                {
                    case DialogResult.Cancel:
                        {
                            return;
                        }

                    case DialogResult.Yes:
                        {
                            if(saveProject() != DialogResult.OK)
                            {
                                return;
                            }
                            break;
                        };
                }
            }

            //finally reset the project
            this.Enabled = false;

            _dirty = false;
            this.pictureBox2.BackgroundImage = null;
            gridImage = null;
            obstaclesImage = null;
            bluetoothImage = null;

            _imgWidth = 0;
            _imgHeight = 0;

            _pixelsPerRow = 20;
            _pixelsPerCol = 20;
            _numCols = 0;
            _numRows = 0;
            _map = null;
            _bt_sensors.Clear(); ;
            _bgImagePath = string.Empty;

            this.Enabled = true;
            
        }

        private DialogResult saveProject()
        {
            try
            {
                if(sfd.ShowDialog() == DialogResult.Cancel)
                {
                    return DialogResult.Cancel;
                }
                string filename = sfd.FileName;

                if (null == _map)
                {
                    return DialogResult.Cancel;
                }
                try
                {
                    Dictionary<string, string> project = new Dictionary<string, string>();
                    project["background"] = _bgImagePath;
                    project["map"] = JsonConvert.SerializeObject(_map);
                    project["btsensors"] = JsonConvert.SerializeObject(_bt_sensors);
                    project["imgwidth"] = "" + _imgWidth;
                    project["imgheight"] = "" + _imgHeight;
                    project["pixelsPerRow"] = "" + _pixelsPerRow;
                    project["pixelsPerCol"] = "" + _pixelsPerCol;
                    project["numCols"] = "" + _numCols;
                    project["numRows"] = "" + _numRows;


                    File.WriteAllText(filename, JsonConvert.SerializeObject(project));

                    _dirty = false;

                    return DialogResult.OK;

                }
                catch (Exception ex)
                {
                    _log.Error(ex);
                }

            }
            catch(Exception ex)
            {
                _log.Error(ex);
            }

            return DialogResult.Cancel;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            reset();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveProject();
        }

        private void setBackgroundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ofd.Filter = "All Graphics Types|*.bmp;*.jpg;*.jpeg;*.png;*.tif;*.tiff"
                + "BMP|*.bmp|GIF|*.gif|JPG|*.jpg;*.jpeg|PNG|*.png|TIFF|*.tif;*.tiff|";
                  
            ofd.FilterIndex = 1;

            if(ofd.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            try
            {
                loadBG(ofd.FileName);
            }
            catch(Exception ex)
            {
                _log.Error(ex);

                MessageBox.Show("There was a problem loading that image.", 
                    "Set Background", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }

        private void loadBG(string imgpath)
        {
            _bgImagePath = imgpath;
            this.pictureBox2.BackgroundImage = Image.FromFile(_bgImagePath);
            _imgWidth = this.pictureBox2.BackgroundImage.Width;
            _imgHeight = this.pictureBox2.BackgroundImage.Height;

            _numCols = _imgWidth / _pixelsPerCol;
            _numRows = _imgHeight / _pixelsPerRow;
            _map = new short[_numCols * _numRows];

            createGridImage();
            createmapImage();
            createBluetoothImage();
        }

        private void chkObstacle_CheckedChanged(object sender, EventArgs e)
        {
            if (chkObstacle.Checked)
            {
                chkClear.Checked = false;
                chkBluetooth.Checked = false;
            }
        }

        private void chkClear_CheckedChanged(object sender, EventArgs e)
        {
            if (chkClear.Checked)
            {
                chkObstacle.Checked = false;
                chkBluetooth.Checked = false;
            }
        }

        private void chkBluetooth_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBluetooth.Checked)
            {
                chkObstacle.Checked = false;
                chkClear.Checked = false;
            }
        }

        private void redrawObstacles()
        {
            try
            {
                Graphics g = Graphics.FromImage(obstaclesImage);
                g.Clear(Color.Transparent);
                
                for(int i = 0;i<_numRows; i++)
                {
                    for(int j = 0; j < _numCols; j++)
                    {
                        if (_map[i * _numCols + j] == (short)Tiles.Obstacle)
                        {
                            g.DrawRectangle(Pens.Red,
                                j * _pixelsPerCol,
                                i * _pixelsPerRow,
                                _pixelsPerCol, _pixelsPerRow);

                            g.FillRectangle(obstacleBrush,
                                j * _pixelsPerCol,
                                i * _pixelsPerRow,
                                _pixelsPerCol, _pixelsPerRow);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                _log.Error(ex);
            }
        }

        private void redrawBluetoothSensors()
        {
            try
            {
                Graphics g = Graphics.FromImage(bluetoothImage);
                g.Clear(Color.Transparent);

                for (int i = 0; i < _numRows; i++)
                {
                    for (int j = 0; j < _numCols; j++)
                    {
                        string index = string.Format("{0}:{1}", i, j);

                        if (_bt_sensors.ContainsKey(index) && !string.IsNullOrEmpty(_bt_sensors[index]))
                        {
                            g.DrawRectangle(Pens.CornflowerBlue,
                                j * _pixelsPerCol,
                                i * _pixelsPerRow,
                                _pixelsPerCol, _pixelsPerRow);

                            g.FillRectangle(bluetoothBrush,
                                j * _pixelsPerCol,
                                i * _pixelsPerRow,
                                _pixelsPerCol, _pixelsPerRow);

                            g.DrawString(
                                    _bt_sensors[index],
                                    fntText,
                                    Brushes.Black,
                                    j * _pixelsPerCol + _pixelsPerCol,
                                    i * _pixelsPerRow + _pixelsPerRow,
                                    StringFormat.GenericDefault);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ofd.Filter = "Find My Car Project Files (*.fmc)|*.fmc";

            ofd.FilterIndex = 1;

            if (ofd.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            try
            {
                Dictionary<string, string> project =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(ofd.FileName));

                _bgImagePath = project["background"];
                loadBG(_bgImagePath);

                _map = JsonConvert.DeserializeObject<short[]>(project["map"]);
                _bt_sensors = JsonConvert.DeserializeObject<Dictionary<string, string>>(project["btsensors"]);

                redrawObstacles();
                redrawBluetoothSensors();

                pictureBox2.Refresh();
            }
            catch (Exception ex)
            {
                _log.Error(ex);

                MessageBox.Show("There was a problem loading that image.",
                    "Set Background",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {

        }

        private void insertSensorAnalysisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ofd.Filter = "Sensor Analysis Files (*.json)|*.json";

            ofd.FilterIndex = 1;

            if (ofd.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            try
            {

                List<Dictionary<string, object>> sensors = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>
                    (File.ReadAllText(ofd.FileName));

                foreach(var sensor in sensors)
                {
                    string index = string.Format("{0}:{1}", sensor["gridRow"], sensor["gridCol"]);
                    _bt_sensors[index] = (string)sensor["addr"];
                }

                redrawBluetoothSensors();

                pictureBox2.Refresh();
            }
            catch (Exception ex)
            {
                _log.Error(ex);

                MessageBox.Show("There was a problem processing the file.",
                    "Load sensor data",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
