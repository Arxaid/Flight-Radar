using Functions;
using System;
using System.Text;
using System.Windows.Forms;

namespace ProjectSQL
{
    public partial class Interface : Form
    {
        TSimulator Simulator;
        int AircraftCounterBase;
        int MissileCounterBase;
        string FirstQuery;
        string SecondQuery;
        string MainQuery;
        public Interface()
        {
            InitializeComponent();
            comboBox2.SelectedIndexChanged += comboBox2_SelectedIndexChanged;
            comboBox6.SelectedIndexChanged += comboBox6_SelectedIndexChanged;
            Simulator = new TSimulator();
            AircraftCounterBase = 0;
            MissileCounterBase = 0;
            FirstQuery = " ";
            SecondQuery = " ";
            MainQuery = " ";
        }

        private String[] GetFields()
        {
            var Items = checkedListBox1.CheckedItems;
            String[] output = new String[Items.Count];

            for (int Counter = 0; Counter < Items.Count; Counter++)
            {
                output[Counter] = Items[Counter].ToString();
            }

            return output;
        }

        private String[] GetFields2()
        {
            var Items = checkedListBox2.CheckedItems;
            String[] output = new String[Items.Count];

            for (int Counter = 0; Counter < Items.Count; Counter++)
            {
                output[Counter] = Items[Counter].ToString();
            }

            return output;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedState = comboBox1.SelectedItem.ToString();
            if(selectedState == "Command post")
            {
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                textBox3.Enabled = false;
                textBox4.Enabled = false;
                textBox5.Enabled = false;
                textBox6.Enabled = true;
                textBox7.Enabled = false;
                textBox8.Enabled = false;
                textBox9.Enabled = false;
                textBox10.Enabled = false;
            }
            if(selectedState == "RLS")
            {
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                textBox3.Enabled = false;
                textBox4.Enabled = false;
                textBox5.Enabled = false;
                textBox6.Enabled = false;
                textBox7.Enabled = true;
                textBox8.Enabled = true;
                textBox9.Enabled = true;
                textBox10.Enabled = true;
            }
            if(selectedState == "Aircraft")
            {
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                textBox3.Enabled = true;
                textBox4.Enabled = false;
                textBox5.Enabled = false;
                textBox6.Enabled = false;
                textBox7.Enabled = false;
                textBox8.Enabled = false;
                textBox9.Enabled = false;
                textBox10.Enabled = false;
            }
            if(selectedState == "Missile")
            {
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                textBox3.Enabled = true;
                textBox4.Enabled = true;
                textBox5.Enabled = true;
                textBox6.Enabled = false;
                textBox7.Enabled = false;
                textBox8.Enabled = false;
                textBox9.Enabled = false;
                textBox10.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double XCoord = Convert.ToDouble(textBox1.Text);
            double YCoord = Convert.ToDouble(textBox2.Text);
            double Velocity = Convert.ToDouble(textBox3.Text);
            double Acceleration = Convert.ToDouble(textBox4.Text);
            double MaxVelocity = Convert.ToDouble(textBox5.Text);
            double SafetyDistance = Convert.ToDouble(textBox6.Text);
            double TargetingDistance = Convert.ToDouble(textBox7.Text);
            double StartingTime = Convert.ToDouble(textBox8.Text);
            double EndingTime = Convert.ToDouble(textBox9.Text);
            double StepTime = Convert.ToDouble(textBox10.Text);

            double CourseCreator(double XCoord, double YCoord)
            {
                double Azimuth = Math.Atan(YCoord / XCoord);
                double Course;
                if (YCoord < 0)
                {
                    Course = -(90 / 57.3) * Azimuth;
                }
                else
                {
                    Course = (90 / 57.3) * Azimuth;
                }
                return Course;
            }
           
            string selectedState = comboBox1.SelectedItem.ToString();
            if(selectedState == "Command post")
            {
                Simulator.ObjectCreator(Models.TargetType.Type.CommandPost, XCoord, YCoord, Velocity, Acceleration, MaxVelocity, SafetyDistance, TargetingDistance, StartingTime, CourseCreator(XCoord, YCoord));
                string[] Columns = new string[] { "CP_XCoord", "CP_YCoord", "SafeDistance" };
                string[] Values = new string[] { XCoord.ToString(), YCoord.ToString(), SafetyDistance.ToString() };
                DBFunctions.DBFunctions.Insert("CommandPost", Columns, Values);
            }
            if(selectedState == "RLS")
            {
                Simulator.ObjectCreator(Models.TargetType.Type.RLS, XCoord, YCoord, Velocity, Acceleration, MaxVelocity, SafetyDistance, TargetingDistance, StartingTime, CourseCreator(XCoord, YCoord));
                string[] Columns = new string[] { "RLS_XCoord", "RLS_YCoord", "Distance" };
                string[] Values = new string[] { XCoord.ToString(), YCoord.ToString(), TargetingDistance.ToString() };
                DBFunctions.DBFunctions.Insert("RLS", Columns, Values);
            }
            if(selectedState == "Aircraft")
            {
                Simulator.ObjectCreator(Models.TargetType.Type.Aircraft, XCoord, YCoord, Velocity, Acceleration, MaxVelocity, SafetyDistance, TargetingDistance, StartingTime, CourseCreator(XCoord, YCoord));
                AircraftCounterBase++;
                string[] Columns = new string[] { "XCoord", "YCoord", "Velocity", "Acceleration", "TT_ID" };
                string[] Values = new string[] { XCoord.ToString(), YCoord.ToString(), Velocity.ToString(), Acceleration.ToString(), "1" };
                DBFunctions.DBFunctions.Insert("Targets", Columns, Values);
            }
            if(selectedState == "Missile")
            {
                Simulator.ObjectCreator(Models.TargetType.Type.Missile, XCoord, YCoord, Velocity, Acceleration, MaxVelocity, SafetyDistance, TargetingDistance, StartingTime, CourseCreator(XCoord, YCoord));
                MissileCounterBase++;
                string[] Columns = new string[] { "XCoord", "YCoord", "Velocity", "Acceleration", "TT_ID" };
                string[] Values = new string[] { XCoord.ToString(), YCoord.ToString(), Velocity.ToString(), Acceleration.ToString(), "2" };
                DBFunctions.DBFunctions.Insert("Targets", Columns, Values);
            }
            if(selectedState == "Unknown")
            {
                Simulator.ObjectCreator(Models.TargetType.Type.Unknown, XCoord, YCoord, Velocity, Acceleration, MaxVelocity, SafetyDistance, TargetingDistance, StartingTime, CourseCreator(XCoord, YCoord));
            }
        }
       
        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Simulator.Run(AircraftCounterBase, MissileCounterBase);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            String[,] Tables = DBFunctions.DBFunctions.ExecuteCustomQuery("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES");

            foreach(var Table in Tables)
            {
                comboBox2.Items.Add(Table.ToString());
                comboBox6.Items.Add(Table.ToString());
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string state = comboBox2.SelectedItem.ToString();
            comboBox3.Items.Clear();
            checkedListBox1.Items.Clear();
            String[,] Columns = DBFunctions.DBFunctions.ExecuteCustomQuery("SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + comboBox2.SelectedItem.ToString() + "'");

            foreach (var Column in Columns)
            {
                comboBox3.Items.Add(Column.ToString());
                checkedListBox1.Items.Add(Column.ToString());
            }
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            string state = comboBox6.SelectedItem.ToString();
            comboBox7.Items.Clear();
            checkedListBox2.Items.Clear();
            String[,] Columns = DBFunctions.DBFunctions.ExecuteCustomQuery("SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + comboBox6.SelectedItem.ToString() + "'");

            foreach (var Column in Columns)
            {
                comboBox7.Items.Add(Column.ToString());
                checkedListBox2.Items.Add(Column.ToString());
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            switch (comboBox4.Items[comboBox4.SelectedIndex].ToString())
            {
                case "Select":
                    string SelectState = " ";
                    string[] SelectFields = GetFields();
                    for(int Counter = 0; Counter < SelectFields.Length - 1; Counter++)
                    {
                        SelectState += (SelectFields[Counter].ToString() + ", ");
                    }
                    SelectState += SelectFields[SelectFields.Length - 1].ToString();

                    sb.AppendFormat("SELECT {0} FROM {1} WHERE {2}{3}{4}",
                                    SelectState, comboBox2.SelectedItem.ToString(), comboBox3.SelectedItem.ToString(), 
                                    comboBox5.SelectedItem.ToString(), textBox11.Text.ToString()
                    );
                    break;
                case "Delete":
                    string DeleteState = " ";
                    string[] DeleteFields = GetFields();
                    for (int Counter = 0; Counter < DeleteFields.Length - 1; Counter++)
                    {
                        DeleteState += (DeleteFields[Counter].ToString() + ", ");
                    }
                    DeleteState += DeleteFields[DeleteFields.Length - 1].ToString();

                    sb.AppendFormat("DELETE {0} FROM {1} WHERE {2}{3}{4}",
                                    DeleteState, comboBox2.SelectedItem.ToString(), comboBox3.SelectedItem.ToString(),
                                    comboBox5.SelectedItem.ToString(), textBox11.Text.ToString()
                    );
                    break;
                default:
                    return;
            }
            FirstQuery = sb.ToString();

            StringBuilder sbm = new StringBuilder();
            switch (comboBox8.Items[comboBox8.SelectedIndex].ToString())
            {
                case "Select":
                    string SelectState = " ";
                    string[] SelectFields = GetFields();
                    for (int Counter = 0; Counter < SelectFields.Length - 1; Counter++)
                    {
                        SelectState += (SelectFields[Counter].ToString() + ", ");
                    }
                    SelectState += SelectFields[SelectFields.Length - 1].ToString();

                    sbm.AppendFormat("SELECT {0} FROM {1} WHERE {2}{3}{4}",
                                    SelectState, comboBox6.SelectedItem.ToString(), comboBox7.SelectedItem.ToString(),
                                    comboBox9.SelectedItem.ToString(), textBox12.Text.ToString()
                    );
                    break;
                case "Delete":
                    string DeleteState = " ";
                    string[] DeleteFields = GetFields();
                    for (int Counter = 0; Counter < DeleteFields.Length - 1; Counter++)
                    {
                        DeleteState += (DeleteFields[Counter].ToString() + ", ");
                    }
                    DeleteState += DeleteFields[DeleteFields.Length - 1].ToString();

                    sbm.AppendFormat("DELETE {0} FROM {1} WHERE {2}{3}{4}",
                                    DeleteState, comboBox6.SelectedItem.ToString(), comboBox7.SelectedItem.ToString(),
                                    comboBox9.SelectedItem.ToString(), textBox12.Text.ToString()
                    );
                    break;
                default:
                    return;
            }
            SecondQuery = sbm.ToString();

            switch (comboBox10.Items[comboBox10.SelectedIndex].ToString())
            {
                case "Union":
                    MainQuery = FirstQuery + " UNION " + SecondQuery;
                    break;
                case "UnionAll":
                    MainQuery = FirstQuery + " UNION ALL " + SecondQuery;
                    break;
                default:
                    return;
            }
            MessageBox.Show(MainQuery);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            switch (comboBox4.Items[comboBox4.SelectedIndex].ToString())
            {
                case "Select":
                    string SelectState = " ";
                    string[] FirstFields = GetFields();
                    string[] SecondFields = GetFields2();
                    for (int Counter = 0; Counter < FirstFields.Length - 1; Counter++)
                    {
                        SelectState += (comboBox2.SelectedItem.ToString() + "." + FirstFields[Counter].ToString() + ", ");
                        SelectState += (comboBox6.SelectedItem.ToString() + "." + SecondFields[Counter].ToString() + ", ");
                    }
                    SelectState += (comboBox2.SelectedItem.ToString() + "." + FirstFields[FirstFields.Length - 1].ToString());
                    SelectState += ", ";
                    SelectState += (comboBox6.SelectedItem.ToString() + "." + SecondFields[FirstFields.Length - 1].ToString());
                    sb.AppendFormat("SELECT {0} FROM {1}",SelectState, comboBox2.SelectedItem.ToString());
                    break;
                default:
                    return;
            }
            FirstQuery = sb.ToString();

            StringBuilder sbm = new StringBuilder();
            switch (comboBox8.Items[comboBox8.SelectedIndex].ToString())
            {
                case "Select":
                    string SelectState = " ";

                    SelectState += (comboBox2.SelectedItem.ToString() + ".ID ");
                    SelectState += "= ";
                    SelectState += (comboBox6.SelectedItem.ToString() + ".ID");

                    sbm.AppendFormat(" {0} ON {1}", comboBox6.SelectedItem.ToString(), SelectState);
                    break;
                default:
                    return;
            }
            SecondQuery = sbm.ToString();

            switch (comboBox10.Items[comboBox10.SelectedIndex].ToString())
            {
                case "Inner":
                    MainQuery = FirstQuery + " INNER JOIN " + SecondQuery;
                    break;
                case "Left":
                    MainQuery = FirstQuery + " LEFT JOIN " + SecondQuery;
                    break;
                case "Right":
                    MainQuery = FirstQuery + " RIGHT JOIN " + SecondQuery;
                    break;
                default:
                    return;
            }
            MessageBox.Show(MainQuery);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            String[,] generatedData = DBFunctions.DBFunctions.ExecuteCustomQuery(MainQuery);
            String[] Fields = GetFields();

            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            foreach(var Field in Fields)
            {
                DataGridViewColumn Column = new DataGridViewTextBoxColumn();
                Column.HeaderText = Field;
                Column.Name = Field;
                Column.ReadOnly = true;
                dataGridView1.Columns.Add(Column);
            }

            for (int i = 0; i < generatedData.Length / (Fields.Length + 1); i++)
            {
                dataGridView1.Rows.Add();
                for (int j = 0; j < Fields.Length; j++)
                {
                    dataGridView1.Rows[i].Cells[j].Value = generatedData[i, j];
                }
            }
        }
    }
}