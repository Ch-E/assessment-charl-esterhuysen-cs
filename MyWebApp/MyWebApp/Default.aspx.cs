using AjaxControlToolkit;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MyWebApp
{
    public partial class _Default : Page
    {
        AccordionPane pane;

        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            DataTable dtModel = new DataTable();

            //Retrieve all rows from DB
            try
            {
                using (SqlConnection connection = new SqlConnection())
                {
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["DataBaseConnectionString"].ToString();
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "Select * from Car ORDER BY Model ASC";
                    cmd.CommandType = CommandType.Text;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    cmd.Dispose();
                    connection.Close();
                }
            }
            catch (Exception)
            {

            }

            //Retrieve all distinct car models from DB - used for accordion header
            try
            {
                using (SqlConnection connection = new SqlConnection())
                {
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["DataBaseConnectionString"].ToString();
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "Select distinct(Model) from Car";
                    cmd.CommandType = CommandType.Text;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dtModel);
                    cmd.Dispose();
                    connection.Close();
                }
            }
            catch (Exception)
            {

            }

            //Instantiate accordion and set properties
            Accordion accCarStock = new Accordion();
            accCarStock.ID = "MyAccordion";
            accCarStock.SelectedIndex = -1;
            accCarStock.RequireOpenedPane = false;
            accCarStock.HeaderCssClass = "accordionHeader";
            accCarStock.HeaderSelectedCssClass = "accordionHeaderSelected";
            accCarStock.ContentCssClass = "accordionContent";
            accCarStock.FramesPerSecond = 60;

            Label lblTitle; Label lblSubModel; Label lblPrice; Label lblSpace; TextBox txtStock; Button btnUpdate;

            //Loop through Car table to add header & content
            for (int i = 0; i < dtModel.Rows.Count; i++)
            {
                //Add header
                string sModel = dtModel.Rows[i]["Model"].ToString();
                pane = new AccordionPane();
                pane.ID = "Pane_" + sModel.ToString();
                lblTitle = new Label();
                lblTitle.Text = sModel;
                pane.HeaderContainer.Controls.Add(lblTitle);

                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    if (sModel == dt.Rows[j]["Model"].ToString())
                    {
                        //Add content
                        lblSubModel = new Label();
                        lblPrice = new Label();
                        lblSpace = new Label();
                        txtStock = new TextBox();
                        pane.ID = "Pane_" + dt.Rows[j]["Sub_Model"].ToString();
                        lblSubModel.Text = "<strong>" + dt.Rows[j]["Sub_Model"].ToString() + "</strong>" + "</br>";
                        lblPrice.Text = "Price: R " + (dt.Rows[j]["Price"].ToString()).Substring(0, 3) + " " + (dt.Rows[j]["Price"].ToString()).Substring(3, 3) + ",00" + "</br>Stock: ";
                        txtStock.ID = dt.Rows[j]["Sub_Model"].ToString();
                        txtStock.Text = dt.Rows[j]["Stock"].ToString();
                        txtStock.Width = 35;
                        lblSpace.Text = "<hr/>";
                        pane.ContentContainer.Controls.Add(lblSubModel);
                        pane.ContentContainer.Controls.Add(lblPrice);
                        pane.ContentContainer.Controls.Add(txtStock);
                        pane.ContentContainer.Controls.Add(lblSpace);
                        accCarStock.Panes.Add(pane);
                        txtStock.TextChanged += new EventHandler(TxtStock_TextChanged);
                    }
                }

                btnUpdate = new Button();
                btnUpdate.ID = "Button_" + dtModel.Rows[i]["Model"].ToString();
                btnUpdate.Text = "Update " + dtModel.Rows[i]["Model"].ToString() + " range";
                btnUpdate.CssClass = "button";
                pane.ContentContainer.Controls.Add(btnUpdate);
                btnUpdate.Click += new EventHandler(btnUpdate_Click);
            }

            //Add accordion to panel
            this.MyContent.Controls.Add(accCarStock);

        }

        private void TxtStock_TextChanged(object sender, EventArgs e)
        {
            int updated; string StockText; int stock = 0; string modelName = "";
            TextBox txtStock = sender as TextBox;

            if (txtStock != null)
            {
                StockText = txtStock.Text;
                modelName = txtStock.ID;

                if (int.TryParse(StockText, out stock) == false)
                {              
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('Please enter a valid number.');", true);
                }
                else
                {
                    try
                    {
                        using (SqlConnection connection = new SqlConnection())
                        {
                            connection.ConnectionString = ConfigurationManager.ConnectionStrings["DataBaseConnectionString"].ToString();
                            connection.Open();
                            SqlCommand cmd = new SqlCommand("UPDATE Car SET Car.Stock = @stock WHERE Car.Sub_Model = @sub_model", connection);
                            cmd.Parameters.AddWithValue("@stock", stock);
                            cmd.Parameters.AddWithValue("@sub_model", modelName);
                            updated = cmd.ExecuteNonQuery();

                            cmd.Dispose();
                            connection.Close();
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection())
                {
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["DataBaseConnectionString"].ToString();
                    connection.Open();


                    connection.Close();
                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('Stock updated successfully.');", true);
            }
            catch (Exception)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('There was an error updating the stock.');", true);
            } 
        }
    }
}