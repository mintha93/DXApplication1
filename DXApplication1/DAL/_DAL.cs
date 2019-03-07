using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using DXApplication1.DTO;

namespace DXApplication1.DAL
{
    class _DAL
    {
        SqlConnection _conn = null;
        public DataTable getTonKho(int Ky, int Nam, int Makho, string Column, int Tanggiam, int Sodong,Double Sothuc)
        {
            _conn = ConSQL.getConnect();
            SqlCommand cmd = new SqlCommand("GET_SETTONKHO", _conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Ky", SqlDbType.Int);
            cmd.Parameters["@Ky"].Value = Ky;
            cmd.Parameters.Add("@Nam", SqlDbType.Int);
            cmd.Parameters["@Nam"].Value = Nam;
            cmd.Parameters.Add("@Makho", SqlDbType.Int);
            cmd.Parameters["@Makho"].Value = Makho;
            cmd.Parameters.Add("@Column", SqlDbType.NVarChar);
            cmd.Parameters["@Column"].Value = Column;
            cmd.Parameters.Add("@Tanggiam", SqlDbType.Int);
            cmd.Parameters["@Tanggiam"].Value = Tanggiam;
            cmd.Parameters.Add("@Sodong", SqlDbType.Int);
            cmd.Parameters["@Sodong"].Value = Sodong;
            cmd.Parameters.Add("@Sothuc", SqlDbType.Float);
            cmd.Parameters["@Sothuc"].Value = Sothuc;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public void updateTonkho(Double Tiennhap, Int64 KHOA, Int64 MAVT, string Column)
        {
            _conn = ConSQL.getConnect();
            SqlCommand Command = new SqlCommand();
            Command = _conn.CreateCommand();
            Command.CommandText = @"UPDATE TONKHO SET " + Column + " = @Tiennhap WHERE KHOA =@KHOA AND MAVT = @MAVT";
            Command.Parameters.AddWithValue("@Tiennhap", Tiennhap);
            Command.Parameters.AddWithValue("@KHOA", KHOA);
            Command.Parameters.AddWithValue("@MAVT", MAVT);
            Command.ExecuteNonQuery();
        }
        public void updateTienDau(Double Tiennhap, int Ky, int Nam, int Makho, Int64 MAVT)
        {
            _conn = ConSQL.getConnect();
            SqlCommand Command = new SqlCommand();
            Command = _conn.CreateCommand();
            Command.CommandText = @"UPDATE TONKHO SET TIEN_DAU = @Tiennhap WHERE KY =@KY AND NAM = @NAM AND MAKHO = @MAKHO AND MAVT = @MAVT";
            Command.Parameters.AddWithValue("@Tiennhap", Tiennhap);
            Command.Parameters.AddWithValue("@KY", Ky);
            Command.Parameters.AddWithValue("@NAM", Nam);
            Command.Parameters.AddWithValue("@MAKHO", Makho);
            Command.Parameters.AddWithValue("@MAVT", MAVT);
            Command.ExecuteNonQuery();

        }
        public Double getSumTotal(String Column,Int16 Ky, Int16 Nam, Int16 Makho)
        {

            _conn = ConSQL.getConnect();
            SqlCommand Command = new SqlCommand();
            Command = _conn.CreateCommand();
            Command.CommandText = @"SELECT SUM("+Column +") FROM TONKHO WHERE KY = "+Ky+" AND NAM = "+Nam+" AND MAKHO = "+Makho+"" ;
            Double Total = (Double)Command.ExecuteScalar();
            return Total;
        }

        /* -----------------------------PDC---------------------------------------------*/
        public DataTable getPDC(String date1, String date2)
        {
            _conn = ConSQL.getConnect();
            SqlCommand Command = new SqlCommand();
            Command = _conn.CreateCommand();
            Command.CommandText = @"SELECT * FROM DIEUCHINH WHERE CONVERT(DATE,NGAY)>= '" + date1 + "' AND CONVERT(DATE,NGAY) <='" + date2 + "'";
            SqlDataAdapter da = new SqlDataAdapter(Command);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
        public DataTable getPDC_CT(string KHOA)
        {
            _conn = ConSQL.getConnect();
            SqlCommand Command = new SqlCommand();
            Command = _conn.CreateCommand();
            Command.CommandText = @"SELECT KHOACT,KHOA,LOC,STT,MAVT,LOAITHUE,THUE_SUAT,GIAGOC,GIANHAP,GIABAN,GIABAN2,GIABAN_NT,DONGIA,DONGIA2,DONGIA_NT,TYGIA 
                                    FROM DIEUCHINH_CT WHERE KHOA= '" + KHOA + "'";
            SqlDataAdapter da = new SqlDataAdapter(Command);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
        public Double getCurval_DieuChinhCT()
        {
            _conn = ConSQL.getConnect();
            SqlCommand Command = new SqlCommand();
            Command = _conn.CreateCommand();
            Command.CommandText = @"SELECT SEQ_CURVAL FROM SYS_SEQUENCE WHERE SEQ_NAME = 'DIEUCHINH_CT'";
            Double Curval = (Double)Command.ExecuteScalar();
            return Curval;
        }

        public DataTable ModifyDetailPDC(GridView grv)
        {
            DataTable dt = new DataTable();

                _conn = ConSQL.getConnect();
                
            for (int i =0; i < grv.RowCount; i++)
            {
                SqlCommand Command = new SqlCommand();
                Command = _conn.CreateCommand();
                Command.CommandText = @"SELECT 0 as KHOACT,0 as KHOA, 0 as LOC,0 as STT,B.MABH as MAVT,C.NHOMTHUE_RA AS LOAITHUE,A.TH_SUAT AS THUE_SUAT,B.GIAGOC,B.GIANHAP,a.GIABAN,a.GIABAN2,ISNULL(a.GIABAN_NT,0) AS GIABAN_NT,
                                        0.0 as DONGIA,0 as DONGIA2,0.0 as DONGIA_NT,ISNULL(a.TYGIA,'0') as TYGIA
                                        FROM DM_HH A join DM_HH_CT B on A.MAVT = B.MAVT
			                                         join DM_LOAITHUE C on	A.LOAITHUE = C.MALT
                                         where b.MABH = '" + grv.GetRowCellValue(i,"MAVT").ToString() + "'";
                SqlDataAdapter da = new SqlDataAdapter(Command);
                DataTable dt1 = new DataTable();
                da.Fill(dt1);
                dt.Merge(dt1);
            }
            return dt;

        }
        public void InsertDC_DT(GridView grv)
        {
            int count = 0;
            for (int i = 0; i < grv.RowCount; i++)
            {
                _conn = ConSQL.getConnect();
                SqlCommand Command = new SqlCommand();
                Command = _conn.CreateCommand();
                Command.CommandText = @"INSERT DIEUCHINH_CT(KHOACT,KHOA,LOC,STT,MAVT,LOAITHUE,THUE_SUAT,GIAGOC,GIANHAP,GIABAN,GIABAN2,GIABAN_NT,DONGIA,	DONGIA2,DONGIA_NT,TYGIA)
                                    VALUES(@KHOACT,@KHOA,@LOC,@STT,@MAVT,@LOAITHUE,@THUE_SUAT,@GIAGOC,@GIANHAP,@GIABAN,@GIABAN2,@GIABAN_NT,@DONGIA,@DONGIA2,@DONGIA_NT,@TYGIA)";
                Command.Parameters.AddWithValue("@KHOACT", grv.GetRowCellValue(i, "KHOACT"));
                Command.Parameters.AddWithValue("@KHOA", grv.GetRowCellValue(i, "KHOA"));
                Command.Parameters.AddWithValue("@LOC", grv.GetRowCellValue(i, "LOC"));
                Command.Parameters.AddWithValue("@STT", grv.GetRowCellValue(i, "STT"));
                Command.Parameters.AddWithValue("@MAVT", grv.GetRowCellValue(i, "MAVT"));
                Command.Parameters.AddWithValue("@LOAITHUE", grv.GetRowCellValue(i, "LOAITHUE"));
                Command.Parameters.AddWithValue("@THUE_SUAT", grv.GetRowCellValue(i, "THUE_SUAT"));
                Command.Parameters.AddWithValue("@GIAGOC", grv.GetRowCellValue(i, "GIAGOC"));
                Command.Parameters.AddWithValue("@GIANHAP", grv.GetRowCellValue(i, "GIANHAP"));
                Command.Parameters.AddWithValue("@GIABAN", grv.GetRowCellValue(i, "GIABAN"));
                Command.Parameters.AddWithValue("@GIABAN2", grv.GetRowCellValue(i, "GIABAN2"));
                Command.Parameters.AddWithValue("@GIABAN_NT", grv.GetRowCellValue(i, "GIABAN_NT"));
                Command.Parameters.AddWithValue("@DONGIA", grv.GetRowCellValue(i, "DONGIA"));
                Command.Parameters.AddWithValue("@DONGIA2", grv.GetRowCellValue(i, "DONGIA2"));
                Command.Parameters.AddWithValue("@DONGIA_NT", grv.GetRowCellValue(i, "DONGIA_NT"));
                Command.Parameters.AddWithValue("@TYGIA", grv.GetRowCellValue(i, "TYGIA"));
                Command.ExecuteNonQuery();
                count++;
            }
            MessageBox.Show("Đã thêm vào " + count.ToString() + " dòng");
        }
        public void updateSys_sequence( String Curval)
        {
            _conn = ConSQL.getConnect();
            SqlCommand Command = new SqlCommand();
            Command = _conn.CreateCommand();
            Command.CommandText = @"UPDATE SYS_SEQUENCE SET SEQ_CURVAL = '"+Curval+"' WHERE SEQ_NAME = 'DIEUCHINH_CT'";

            Command.ExecuteNonQuery();
        }
    }
}
