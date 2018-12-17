using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraGrid.Views.Grid;
using DXApplication1.DAL;
namespace DXApplication1.BUS
{
    class _BUS
    {
        _DAL _DAL = new _DAL();
        public DataTable getTonKho(int Ky, int Nam, int Makho, string Column, int Tanggiam, int Sodong)
        {
            return _DAL.getTonKho(Ky, Nam, Makho, Column, Tanggiam, Sodong);
        }
        public void updateTonkho(Double Tiennhap, Int64 KHOA, Int64 MAVT, string Column)
        {
            _DAL.updateTonkho(Tiennhap, KHOA, MAVT, Column);
        }
        public void updateTienDau(Double Tiennhap, int Ky, int Nam, int Makho, Int64 MAVT)
        {
            _DAL.updateTienDau(Tiennhap, Ky, Nam, Makho, MAVT);
        }
        /*---------------------PDC-----------------------------*/
        public DataTable getPDC(String date1, String date2)
        {
            return _DAL.getPDC(date1, date2);
        }
        public DataTable getPDC_CT(string KHOA)
        {
            return _DAL.getPDC_CT(KHOA);
        }
        public Double getCurval_DieuChinhCT()
        {
            return _DAL.getCurval_DieuChinhCT();
        }
        public DataTable ModifyDetailPDC(GridView grv)
        {
            return _DAL.ModifyDetailPDC(grv);
        }
        public void InsertDC_DT(GridView grv)
        {
            _DAL.InsertDC_DT(grv);
        }
        public void updateSys_sequence(String Curval)
        {
            _DAL.updateSys_sequence(Curval);
        }
        public Double getSumTotal(String Column, Int16 Ky, Int16 Nam, Int16 Makho)
        {
            return _DAL.getSumTotal(Column,Ky,Nam,Makho);
        }
     }
}