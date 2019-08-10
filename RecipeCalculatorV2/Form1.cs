using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RecipeCalculatorV2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Label2_Click(object sender, EventArgs e)
        {

        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void PictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void Label6_Click(object sender, EventArgs e)
        {

        }

        private void Label15_Click(object sender, EventArgs e)
        {

        }

        private void Label16_Click(object sender, EventArgs e)
        {

        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(result.Text) || string.IsNullOrWhiteSpace(material1.Text) || string.IsNullOrWhiteSpace(material2.Text) || string.IsNullOrWhiteSpace(material3.Text) || string.IsNullOrWhiteSpace(amount1.Text) || string.IsNullOrWhiteSpace(amount2.Text) || string.IsNullOrWhiteSpace(amount3.Text))
            {
                MessageBox.Show("Please enter all the values.");
                return;
            }
            
            //End result selling price
            double goldenDaric_Price = double.Parse(result.Text);
            //Ingredients buying price
            double goldenTalent_Price = double.Parse(material1.Text);
            double craftKit_Price = double.Parse(material2.Text);
            double elinuTear_Price = double.Parse(material3.Text);
            //Amounts present in the user's inventory
            double materialA = double.Parse(amount1.Text);
            double materialB = double.Parse(amount2.Text);
            double materialC = double.Parse(amount3.Text);

            //if "Include Production Points in calculation" checkbox is checked.
            if (!checkBox1.Checked)
            {
                elinuTear_Price = 0;
            }

            //if "show missing materials" checkbox is checked.
            if (checkBox2.Checked)
            {
                /* My goal here is to find out which material the player has most of relative to the recipe ratios.
                 * 
                 * The ratios are as followed:
                 * Material A (Golden Talent) 5
                 * Material B (Craft Kits) 60
                 * Material C (Production Points) 20
                 * 
                 * Following that, I divide each number by the number needed for to craft one produce (5, 60 and 20).
                 * Whichever value ends up being the highest is now the goal for the other materials to be stocked up to.
                 * For example: Player has enough mats C to craft 60 produce. Mats A and B will subsequently be stocked up to get to 60.
                 */

                //Aux is short for auxiliary calculation don't sue me
                double auxA = materialA / 5;
                double auxB = materialB / 60;
                double auxC = materialC / 20;

                //Rounding down to assure even numbers and that the highest number of mats does not need increasing. 
                //This might cause you to have leftovers of the material that you have most of.
                //Yes, this is messy but sue me.
                int auxARounded = Convert.ToInt32(auxA);
                int auxBRounded = Convert.ToInt32(auxB);
                int auxCRounded = Convert.ToInt32(auxC);

                if (auxARounded > auxBRounded & auxARounded > auxCRounded)
                {
                    //Calculating the need of material B; Max amount of produce according to material A being the highest quantitative standard
                    //minus the material B's you already have = the material B's you still need.
                    double materialBNeed = Convert.ToDouble(auxARounded) * 60 - materialB;
                    double materialCNeed = Convert.ToDouble(auxARounded) * 20 - materialC;
                    missingMaterialB.Text = ($"{materialBNeed}");
                    missingMaterialC.Text = ($"{materialCNeed}");
                }

                else if (auxBRounded > auxARounded & auxBRounded > auxCRounded)
                {
                    double materialANeed = Convert.ToDouble(auxBRounded) * 5 - materialA;
                    double materialCNeed = Convert.ToDouble(auxBRounded) * 20 - materialC;
                    missingMaterialA.Text = ($"{materialANeed}");
                    missingMaterialC.Text = ($"{materialCNeed}");
                }

                else if (auxCRounded > auxARounded & auxCRounded > auxBRounded)
                {
                    double materialANeed = Convert.ToDouble(auxCRounded) * 5 - materialA;
                    double materialBNeed = Convert.ToDouble(auxCRounded) * 60 - materialB;
                    missingMaterialA.Text = ($"{materialANeed}");
                    missingMaterialB.Text = ($"{materialBNeed}");
                }

                //---*--- RARE-ish EXCEPTIONS ---*---
                //All of them can produce the same amount of produce or there is no produce that can be crafted
                else if (auxARounded.Equals(auxBRounded & auxCRounded))
                {
                    return;
                }
                //A equals B and both are bigger than C
                else if (auxARounded.Equals(auxBRounded) & auxARounded > auxCRounded)
                {
                    double materialCNeed = Convert.ToDouble(auxARounded) * 20 - materialC;
                    missingMaterialC.Text = ($"{materialCNeed}");
                }
                //A equals C and both are bigger than B
                else if (auxARounded.Equals(auxCRounded) & auxARounded > auxBRounded)
                {
                    double materialBNeed = Convert.ToDouble(auxARounded) * 60 - materialB;
                    missingMaterialB.Text = ($"{materialBNeed}");
                }
                //B equals C and both are bigger than A
                else if (auxBRounded.Equals(auxCRounded) & auxBRounded > auxARounded)
                {
                    double materialANeed = Convert.ToDouble(auxARounded) * 5 - materialA;
                    missingMaterialA.Text = ($"{materialANeed}");
                }
                else
                {
                    return;
                }
                var profit = 0.99 * 3 * goldenDaric_Price - 5 * goldenTalent_Price - 60 * craftKit_Price - 20 * elinuTear_Price / 4000;
                profitSingle.Text = profit.ToString();
                if (profit >= 0)
                {
                    profitSingle.ForeColor = Color.Green;
                    profitTotal.ForeColor = Color.Green;
                }
                else
                {
                    profitSingle.ForeColor = Color.Red;
                    profitTotal.ForeColor = Color.Red;
                }
                if (auxARounded > auxBRounded & auxARounded > auxCRounded)
                {
                    var profitAll = profit * auxARounded;
                    profitTotal.Text = profitAll.ToString();
                    craftableAmount.Text = $"{auxARounded * 3} if you buy all the missing ingredients.";
                }
                if (auxBRounded > auxARounded & auxBRounded > auxCRounded)
                {
                    var profitAll = profit * auxBRounded;
                    profitTotal.Text = profitAll.ToString();
                    craftableAmount.Text = $"{auxBRounded * 3} if you buy all the missing ingredients.";
                }
                else
                {
                    var profitAll = profit * auxCRounded;
                    profitTotal.Text = profitAll.ToString();
                    craftableAmount.Text = $"{auxCRounded*3} if you buy all the missing ingredients.";
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void PictureBox14_Click(object sender, EventArgs e)
        {

        }

        private void PictureBox8_Click(object sender, EventArgs e)
        {

        }

        private void PictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
