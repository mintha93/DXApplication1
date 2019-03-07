using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DXApplication1.BUS;

namespace DXApplication1
{
    public partial class frmDCG : DevExpress.XtraEditors.XtraForm
    {
        public frmDCG()
        {
            InitializeComponent();
        }
        string addressFile;
        _BUS _BUS = new _BUS();

        private void frmDCG_Load(object sender, EventArgs e)
        {
            try
            {
                labelControl4.Text = _BUS.getCurval_DieuChinhCT().ToString();
                gridCtrlPDC.DataSource = null;
                grvPDC.Columns.Clear();
                gridCtrlPDC.DataSource = _BUS.getPDC(dePDC1.DateTime.ToString("yyyy-MM-dd"), dePDC2.DateTime.ToString("yyyy-MM-dd"));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void grvPDC_DoubleClick(object sender, EventArgs e)
        {
            xtraTabControl1.SelectedTabPage = xtpPDC_CT;
            gridCtrlPDC_CT.DataSource = _BUS.getPDC_CT(grvPDC.GetFocusedRowCellDisplayText("KHOA"));
        }
        private void xtraTabControl1_SelectedPageChanged(object sender, EventArgs e)
        {
          
        }


        private void hyperlinkLabelControl1_Click(object sender, EventArgs e)
        {
            var myForm = new frmSetTonKho();
            myForm.Show();
        }
        private void btnChoosefile_Click(object sender, EventArgs e)
        {
            try
            {
                oFD.Filter = "Tất Cả Các File |*.*|Excel 2003 Files |*.xls|Excel 2007 File|*.xlsx";
                oFD.FileName = "";
                oFD.ShowDialog();
                string connec = string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=""Excel 8.0;HDR=YES;IMEX=1;""", addressFile);
                string query = string.Format("Select * from [{0}]", "Sheet1$");
                OleDbDataAdapter adapter = new OleDbDataAdapter(query, connec);
                DataTable tbSinhVien = new DataTable();
                adapter.Fill(tbSinhVien);
                labelControl4.Text = _BUS.getCurval_DieuChinhCT().ToString();
                if (tbSinhVien.Rows.Count > 0)
                {
                    gridCtrlPDC_CT.DataSource = tbSinhVien;
                }
                else
                {
                    gridCtrlPDC_CT.DataSource = null;
                }
                xtraTabControl1.SelectedTabPage = xtpPDC_CT;
            }
            catch 
            {
                MessageBox.Show("Bạn phải chọn file excel");
            }
        }
        private List<string> getListSheet(string urlFile)
        {
            try
            {
                List<string> sheets = new List<string>();
                string connec = string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=""Excel 8.0;HDR=YES;IMEX=1;""", urlFile);
                DbProviderFactory factory = DbProviderFactories.GetFactory("System.Data.OleDb");
                DbConnection connection = factory.CreateConnection();
                connection.ConnectionString = connec;
                connection.Open();
                DataTable dt = connection.GetSchema("Tables");
                connection.Close();
                foreach (DataRow row in dt.Rows)
                {
                    string sheetnames = (string)row["TABLE_NAME"];
                    sheets.Add(sheetnames);
                }
                return sheets;
           
            }
            catch (Exception)
            {
                return null;
            }
              
        }

        private void oFD_FileOk(object sender, CancelEventArgs e)
        {
            addressFile = oFD.FileName;
            List<string> sheets = getListSheet(addressFile);
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            try
            {
                xtraTabControl1.SelectedTabPage = xtpAddDetail;
                gridAddDetail.DataSource = _BUS.ModifyDetailPDC(grvPDC_CT);
                labelControl4.Text = _BUS.getCurval_DieuChinhCT().ToString();
                Int32 curval =Convert.ToInt32(_BUS.getCurval_DieuChinhCT());
                    for (int i = 0; i <=grvPDC_CT.RowCount- 1; i++)
                    {
                    grvAddDetail.SetRowCellValue(i, "DONGIA2", grvPDC_CT.GetRowCellValue(i, "DONGIA2"));
                    grvAddDetail.SetRowCellValue(i,"DONGIA", Math.Round(Convert.ToDecimal(grvAddDetail.GetRowCellValue(i, "DONGIA2").ToString()) / (1 + Convert.ToDecimal(grvAddDetail.GetRowCellValue(i, "THUE_SUAT")) / 100), 2));
                    grvAddDetail.SetRowCellValue(i, "DONGIA_NT", Math.Round(Convert.ToDecimal(grvAddDetail.GetRowCellValue(i, "DONGIA2").ToString()) / 23300, 2));
                    grvAddDetail.SetRowCellValue(i, "KHOACT", i+1+curval);
                    grvAddDetail.SetRowCellValue(i, "KHOA", grvPDC.GetFocusedRowCellDisplayText("KHOA"));
                    grvAddDetail.SetRowCellValue(i, "LOC", 10);
                    grvAddDetail.SetRowCellValue(i, STT, i + 1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                _BUS.InsertDC_DT(grvAddDetail);
                _BUS.updateSys_sequence(grvAddDetail.GetRowCellValue(grvAddDetail.RowCount - 1, "KHOACT").ToString());
                labelControl4.Text = _BUS.getCurval_DieuChinhCT().ToString();
                //MessageBox.Show(grvAddDetail.GetRowCellValue(grvAddDetail.RowCount - 1, "KHOACT").ToString());
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dePDC1_EditValueChanged(object sender, EventArgs e)
        {
            gridCtrlPDC.DataSource = null;
            grvPDC.Columns.Clear();
            gridCtrlPDC.DataSource = _BUS.getPDC(dePDC1.DateTime.ToString("yyyy-MM-dd"), dePDC2.DateTime.ToString("yyyy-MM-dd"));
        }

        private void dePDC2_EditValueChanged(object sender, EventArgs e)
        {
            gridCtrlPDC.DataSource = null;
            grvPDC.Columns.Clear();
            gridCtrlPDC.DataSource = _BUS.getPDC(dePDC1.DateTime.ToString("yyyy-MM-dd"), dePDC2.DateTime.ToString("yyyy-MM-dd"));
        }


    }
}
