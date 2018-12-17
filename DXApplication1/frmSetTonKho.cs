using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DXApplication1.BUS;

namespace DXApplication1
{
    public partial class frmSetTonKho : DevExpress.XtraEditors.XtraForm
    {
        _BUS _BUS = new _BUS();
        public frmSetTonKho()
        {
            InitializeComponent();
        }

        private void labelControl1_Click(object sender, EventArgs e)
        {
            
        }

        private void btngetData_Click(object sender, EventArgs e)
        {
            try
            {
                gridBefore.DataSource = null;
                grvBefore.Columns.Clear();
                gridAfter.DataSource = null;
                grvAfter.Columns.Clear();
                gridBefore.DataSource = _BUS.getTonKho(Convert.ToInt16(cbeKy.Text), Convert.ToInt16(cbeNam.Text), Convert.ToInt16(textMakho.Text), cbeColumn.Text, Convert.ToInt16(cbeTanggiam.Text), Convert.ToInt16(textSodong.Text));
                btnSumtotal.Text = _BUS.getSumTotal(cbeColumn.Text, Convert.ToInt16(cbeKy.Text), Convert.ToInt16(cbeNam.Text), Convert.ToInt16(textMakho.Text)).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            gridAfter.DataSource = null;
            grvAfter.Columns.Clear();
            gridAfter.DataSource = _BUS.getTonKho(Convert.ToInt16(cbeKy.Text), Convert.ToInt16(cbeNam.Text), Convert.ToInt16(textMakho.Text), cbeColumn.Text, Convert.ToInt16(cbeTanggiam.Text), Convert.ToInt16(textSodong.Text));

            string column3 = grvAfter.Columns[2].ToString();
            if (cbeTanggiam.Text =="0")
                for (int i = 0; i < grvAfter.DataRowCount; i++)
                {
                    Double val = Convert.ToDouble(grvAfter.GetRowCellValue(i, column3)) - 0.4;
                    grvAfter.SetRowCellValue(i, column3, val);
                }
            else if (cbeTanggiam.Text == "1")
                for (int i = 0; i < grvAfter.DataRowCount; i++)
                {
                    Double val = Convert.ToDouble(grvAfter.GetRowCellValue(i, column3)) + 0.4;
                    grvAfter.SetRowCellValue(i, column3, val);
                }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string column3 = grvAfter.Columns[2].ToString();
            int count = 0;
            int count1 = 0;
            for (int i = 0; i < grvAfter.DataRowCount; i++)
            {

                try
                {
                    _BUS.updateTonkho(Convert.ToDouble(grvAfter.GetRowCellValue(i, column3).ToString()), Convert.ToInt64(grvAfter.GetRowCellValue(i, "KHOA").ToString()), Convert.ToInt64(grvAfter.GetRowCellValue(i, "MAVT").ToString()), grvAfter.Columns[2].ToString());
                    count++;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            MessageBox.Show("Đã update " + count.ToString() + " Records bảng " + cbeColumn.Text);
            if (cbeColumn.Text == "TIEN_CUOI")
            {
                int Ky = Convert.ToInt16(cbeKy.Text);
                int Nam = Convert.ToInt16(cbeNam.Text);
                if (Ky == 12)
                {
                    for (int i = 0; i < grvAfter.DataRowCount; i++)
                    { 
                        try
                        {
                            _BUS.updateTienDau(Convert.ToDouble(grvAfter.GetRowCellValue(i, column3).ToString()), 1, Nam+1, Convert.ToInt16(textMakho.Text), Convert.ToInt64(grvAfter.GetRowCellValue(i, "MAVT").ToString()));
                            count1++;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < grvAfter.DataRowCount; i++)
                    { 
                        try
                        {
                            _BUS.updateTienDau(Convert.ToDouble(grvAfter.GetRowCellValue(i, column3).ToString()), Ky+1, Nam, Convert.ToInt16(textMakho.Text), Convert.ToInt64(grvAfter.GetRowCellValue(i, "MAVT").ToString()));
                            count1++;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
             MessageBox.Show("Đã update " + count1.ToString() + " Records bảng TIEN_DAU");
            }

            btnSumtotal.Text = _BUS.getSumTotal(cbeColumn.Text, Convert.ToInt16(cbeKy.Text), Convert.ToInt16(cbeNam.Text), Convert.ToInt16(textMakho.Text)).ToString();

        }

    }
}