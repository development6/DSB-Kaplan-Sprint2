using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kinesiologia.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        [HttpPost]
        public ContentResult AjaxMethod(string paciente)
        {
            string query = "select Replace(Ficha_Kinesiologia.ergo_vol_ing, ',' , '.'), Replace(Ficha_Kinesiologia.ergo_voml_ing, ',' , '.'), Replace(Ficha_Kinesiologia.ergo_fcmax_ing, ',' , '.'), Replace(Ficha_Kinesiologia.ergo_pulso_ing, ',' , '.'), Replace(Ficha_Kinesiologia.ergo_ve_ing, ',' , '.'), Replace(Ficha_Kinesiologia.ergo_mets_ing, ',' , '.'), Replace(Ficha_Kinesiologia.ergo_vol_egr, ',' , '.'), Replace(Ficha_Kinesiologia.ergo_voml_egr, ',' , '.'), Replace(Ficha_Kinesiologia.ergo_fcmax_egr, ',' , '.'), Replace(Ficha_Kinesiologia.ergo_pulso_egr, ',' , '.'), Replace(Ficha_Kinesiologia.ergo_ve_egr, ',' , '.'), Replace(Ficha_Kinesiologia.ergo_mets_egr, ',' , '.'), COUNT(Ficha_Kinesiologia.id_ficha_kine)";
            query += "FROM  Ficha INNER JOIN  Ficha_Kinesiologia ON Ficha_Kinesiologia.id_ficha=Ficha.id_ficha inner join Paciente on Paciente.id_paciente = Ficha.id_paciente where Paciente.id_paciente = @IdPaciente group by Ficha_Kinesiologia.ergo_vol_ing, Ficha_Kinesiologia.ergo_voml_ing, Ficha_Kinesiologia.ergo_fcmax_ing, Ficha_Kinesiologia.ergo_pulso_ing, Ficha_Kinesiologia.ergo_ve_ing, Ficha_Kinesiologia.ergo_mets_ing, Ficha_Kinesiologia.ergo_vol_egr, Ficha_Kinesiologia.ergo_voml_egr, Ficha_Kinesiologia.ergo_fcmax_egr, Ficha_Kinesiologia.ergo_pulso_egr, Ficha_Kinesiologia.ergo_ve_egr, Ficha_Kinesiologia.ergo_mets_egr";
            //string query = "select K.ergo_vol_ing, K.ergo_voml_ing, K.ergo_fcmax_ing, K.ergo_vol_egr, K.ergo_voml_egr, K.ergo_fcmax_egr, COUNT(K.id_ficha_kine)";
            //query += "FROM Ficha_Kinesiologia K, Ficha F, Paciente Pa Where K.id_ficha = F.id_ficha And F.id_paciente ='" + paciente + "'group by K.ergo_vol_ing, K.ergo_voml_ing, K.ergo_fcmax_ing, K.ergo_vol_egr, K.ergo_voml_egr, K.ergo_fcmax_egr";

            string constr = ConfigurationManager.ConnectionStrings["ConexionKaplan"].ConnectionString;
            StringBuilder sb = new StringBuilder();
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@IdPaciente", paciente);
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        sb.Append("[");

                        while (sdr.Read())
                        {
                            sb.Append("{");
                            System.Threading.Thread.Sleep(50);
                            string color = String.Format("#{0:X6}", new Random().Next(0x1000000));
                            sb.Append(string.Format("value0:{0},value1:{1},value2:{2},value3:{3},value4:{4},value5:{5},value6:{6},value7:{7},value8:{8},value9:{9},value10:{10},value11:{11}", sdr[0], sdr[1], sdr[2], sdr[3], sdr[4], sdr[5], sdr[6], sdr[7], sdr[8], sdr[9], sdr[10], sdr[11]));
                            sb.Append("},");
                        }

                        sb = sb.Remove(sb.Length - 1, 1);
                        sb.Append("]");


                        //sb.Append("}");
                    }

                    con.Close();
                }
            }

            return Content(sb.ToString());
        }




    }
}