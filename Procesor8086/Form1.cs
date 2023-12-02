using Microsoft.Win32;
using System.Dynamic;

namespace Procesor8086
{
    public partial class RergisterInput : Form
    {
        sbyte Register1index;
        sbyte Register2index;

        public RergisterInput()
        {
            InitializeComponent();
        }
        private void btnExecute_Click(object sender, EventArgs e)
        {//funkcja wykonywana po klikniêciu przycisku Execute
            if (cRegister1.GetItemText(cRegister1.SelectedItem) != "")//jeœli pierwszy rejestr jest wybrany
                switch (cCommand.GetItemText(cCommand.SelectedItem))
                {
                    case "MOV"://jeœli wybrano polecenie dwuargumentowe
                    case "XCHG":
                    case "ADD":
                    case "SUB":
                    case "AND":
                    case "OR":
                    case "XOR":
                        if (cRegister2.GetItemText(cRegister2.SelectedItem) != "")//jeœli drugi rejestr jest wybrany
                            switch (cCommand.GetItemText(cCommand.SelectedItem))
                            {//switch dla poleceñ dwuargumentowych
                                case "MOV":
                                    MOV(Register1index, Register2index);
                                    break;
                                case "XCHG":
                                    XCHG(Register1index, Register2index);
                                    break;
                                case "ADD":
                                    ADD(Register1index, Register2index);
                                    break;
                                case "SUB":
                                    SUB(Register1index, Register2index);
                                    break;
                                case "AND":
                                    AND(Register1index, Register2index);
                                    break;
                                case "OR":
                                    OR(Register1index, Register2index);
                                    break;
                                case "XOR":
                                    XOR(Register1index, Register2index);
                                    break;
                            }
                        else NoRegister(false);//wyœwietlanie b³êdu jeœli nie wybrano drugiego rejestru
                        break;
                    case "INC"://reszta poleceñ polecenia jednoargumentowe
                        INC(Register1index);
                        break;
                    case "DEC":
                        DEC(Register1index);
                        break;
                    case "NOT":
                        NOT(Register1index);
                        break;
                    case "NEG":
                        NEG(Register1index);
                        break;
                    case "MUL":
                        MUL(Register1index);
                        break;
                    case "IMUL":
                        IMUL(Register1index);
                        break;
                    case "DIV":
                        DIV(Register1index);
                        break;
                    case "IDIV":
                        IDIV(Register1index);
                        break;
                }
            else NoRegister(true);//wyœwietlanie b³êdu jeœli nie wybrano pierwszego rejestru
        }
        public byte FromHex(string hex)
        {
            hex = hex.ToUpper();
            byte number = 0;
            if (hex.Length == 2)
                for (int i = 0; i < hex.Length; i++)
                    if (hex[i] >= 'A' && hex[i] <= 'F') number += (byte)(Math.Pow(16, 1 - i) * (hex[i] - 'A' + 10));
                    else if (hex[i] >= '0' && hex[i] <= '9') number += (byte)(Math.Pow(16, 1 - i) * (hex[i] - '0'));
            return number;
        }
        public byte FromBinary(string binary)
        {
            byte number = 0;
            if (binary != null)
                for (int i = 0; i < binary.Length; i++)
                    if (binary[i] == '0') ;
                    else if (binary[i] == '1') number += (byte)Math.Pow(2, binary.Length - i - 1);
                    else return 0;
            return number;
        }
        public sbyte FromBinarySigned(string binary)
        {
            sbyte number = (sbyte)FromBinary(binary);
            return number;
        }
        public string ToBinary(int l)
        {
            string s = "";
            byte b = (byte)(l % 256);
            for (int i = 0; i < 8; i++)
            {
                if (Math.Pow(2, 7 - i) <= b)
                {
                    s += '1';
                    b -= (byte)Math.Pow(2, 7 - i);
                }
                else s += '0';
            }
            return s;
        }
        public string ToBinarySigned(sbyte l)
        {
            string s = "";
            byte b = (byte)l;
            return ToBinary(b);
        }
        public string IsBinaryToBinary(sbyte Register1)
        {
            if (ReturnText(Register1).Text.Length == 2) return ToBinary(FromHex(ReturnText(Register1).Text));
            else if (ReturnText(Register1).Text.Length == 8) return ReturnText(Register1).Text;
            else
            {
                InputError();
                return null;
            }
        }
        public void InputError()
        {
            MessageBox.Show("Invalid entry in the register you are trying to use! Use binary or hexadecimal notation.");
        }
        public void NoRegister(bool a)
        {
            string s = "";
            if (a) s += "First";
            else s += "Second";
            s += " register is not choosen";
            MessageBox.Show(s);
        }
        public void MOV(sbyte Register1, sbyte Register2)
        {
            ReturnText(Register2).Text = ReturnText(Register1).Text;
        }
        public void XCHG(sbyte Register1, sbyte Register2)
        {
            string temp = ReturnText(Register2).Text;
            ReturnText(Register2).Text = ReturnText(Register1).Text;
            ReturnText(Register1).Text = temp;
        }
        public void ADD(sbyte Register1, sbyte Register2)
        {
            ReturnText(Register1).Text = ToBinary(FromBinary(IsBinaryToBinary(Register1)) + FromBinary(IsBinaryToBinary(Register2)));
        }
        public void SUB(sbyte Register1, sbyte Register2)
        {
            NEG(Register2);
            ADD(Register1, Register2);
        }
        public void AND(sbyte Register1, sbyte Register2)
        {
            string a = IsBinaryToBinary(Register1);
            string b = IsBinaryToBinary(Register2);
            string s = "";
            for (int i = 0; i < 8; i++)
            {
                if (a[i] == '1' && b[i] == '1') s += '1';
                else if (a[i] == '0' || b[i] == '0') s += "0";
                else InputError();
            }
            ReturnText(Register1).Text = s;
        }
        public void OR(sbyte Register1, sbyte Register2)
        {
            string a = IsBinaryToBinary(Register1);
            string b = IsBinaryToBinary(Register2);
            string s = "";
            for (int i = 0; i < 8; i++)
            {
                if (a[i] == '1' || b[i] == '1') s += '1';
                else if (a[i] == '0' && b[i] == '0') s += "0";
                else InputError();
            }
            ReturnText(Register1).Text = s;
        }
        public void XOR(sbyte Register1, sbyte Register2)
        {
            string a = IsBinaryToBinary(Register1);
            string b = IsBinaryToBinary(Register2);
            string s = "";
            for (int i = 0; i < 8; i++)
            {
                if ((a[i] == '1' && b[i] == '0') || (a[i] == '0' && b[i] == '1')) s += '1';
                else if (a[i] == b[i]) s += "0";
                else InputError();
            }
            ReturnText(Register1).Text = s;
        }
        public void INC(sbyte Register1)
        {
            ReturnText(Register1).Text = ToBinary(FromBinary(IsBinaryToBinary(Register1)) + 1);
        }
        public void DEC(sbyte Register1)
        {
            ReturnText(Register1).Text = ToBinary(FromBinary(IsBinaryToBinary(Register1)) - 1);
        }
        public void NOT(sbyte Register1)
        {
            string a = IsBinaryToBinary(Register1);
            string s = "";
            for (int i = 0; i < 8; i++)
            {
                if (a[i] == '1') s += '0';
                else if (a[i] == '0') s += '1';
                else InputError();
            }
            ReturnText(Register1).Text = s;
        }
        public void NEG(sbyte Register1)
        {
            NOT(Register1);
            INC(Register1);
        }
        public void MUL(sbyte Register1)
        {
            int i = FromBinary(IsBinaryToBinary(Register1));
            int sum = 0;
            int reg1 = FromBinary(IsBinaryToBinary(1));
            while (i-- > 0) sum += reg1;
            //musi zwracac do dwóch bytów ah al
            ReturnText(1).Text = ToBinary(sum % 256);
            ReturnText(0).Text = ToBinary(sum / 256);
        }
        public void IMUL(sbyte Register1)
        {
            int i = FromBinarySigned(IsBinaryToBinary(Register1));
            int sum = 0;
            if (i >= 0)
            {
                while (i-- > 0) sum += FromBinarySigned(IsBinaryToBinary(1));
                //musi zwracac do dwóch bytów ah al
                //MessageBox.Show(sum.ToString());
                ReturnText(1).Text = ToBinarySigned((sbyte)(sum % 256));
                ReturnText(0).Text = ToBinarySigned((sbyte)(sum / 256));
            }
            else
            {
                i = -i;
                while (i-- > 0) sum += FromBinarySigned(IsBinaryToBinary(1));
                //musi zwracac do dwóch bytów ah al
                //MessageBox.Show(sum.ToString());
                ReturnText(1).Text = ToBinarySigned((sbyte)(-sum % 256));
                ReturnText(0).Text = ToBinarySigned((sbyte)(-sum / 256));
            }
        }
        public void DIV(sbyte Register1)
        {
            if (Zero(IsBinaryToBinary(Register1)))
            {
                string s = ToBinary(FromBinary(IsBinaryToBinary(1)) % FromBinary(IsBinaryToBinary(Register1)));
                ReturnText(1).Text = ToBinary(FromBinary(IsBinaryToBinary(1)) / FromBinary(IsBinaryToBinary(Register1)));
                ReturnText(0).Text = s;
            }
        }
        public void IDIV(sbyte Register1)
        {
            if (Zero(IsBinaryToBinary(Register1)))
            {
                string s = ToBinary(FromBinarySigned(IsBinaryToBinary(1)) % FromBinarySigned(IsBinaryToBinary(Register1)));
                ReturnText(1).Text = ToBinary(FromBinarySigned(IsBinaryToBinary(1)) / FromBinarySigned(IsBinaryToBinary(Register1)));
                ReturnText(0).Text = s;
            }
        }
        public bool Zero(string binary)
        {
            if(binary== "00000000")
            {
                MessageBox.Show("Do not divide by zero!");
                return false;
            }
            return true;
        }
        public TextBox ReturnText(sbyte temp)
        {
            TextBox cb = null;
            switch (temp)
            {
                case 0:
                    cb = txtAH;
                    break;
                case 1:
                    cb = txtAL;
                    break;
                case 2:
                    cb = txtBH;
                    break;
                case 3:
                    cb = txtBL;
                    break;
                case 4:
                    cb = txtCH;
                    break;
                case 5:
                    cb = txtCL;
                    break;
                case 6:
                    cb = txtDH;
                    break;
                case 7:
                    cb = txtDL;
                    break;
            }

            return cb;
        }
        private void cCommand_SelectionChangeCommitted(object sender, EventArgs e)
        {
            switch (cCommand.GetItemText(cCommand.SelectedItem))
            {
                case "MOV":
                case "XCHG":
                case "ADD":
                case "SUB":
                case "AND":
                case "OR":
                case "XOR":
                    cRegister2.Enabled = true;
                    break;
                case "INC":
                case "DEC":
                case "NOT":
                case "NEG":
                case "MUL":
                case "IMUL":
                case "DIV":
                case "IDIV":
                    cRegister2.Enabled = false;
                    break;
            }
        }
        private void cRegister1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            switch (cRegister1.GetItemText(cRegister1.SelectedItem))
            {
                case "AH":
                    Register1index = 0;
                    break;
                case "AL":
                    Register1index = 1;
                    break;
                case "BH":
                    Register1index = 2;
                    break;
                case "BL":
                    Register1index = 3;
                    break;
                case "CH":
                    Register1index = 4;
                    break;
                case "CL":
                    Register1index = 5;
                    break;
                case "DH":
                    Register1index = 6;
                    break;
                case "DL":
                    Register1index = 7;
                    break;
            }
        }
        private void cRegister2_SelectionChangeCommitted(object sender, EventArgs e)
        {
            switch (cRegister2.GetItemText(cRegister2.SelectedItem))
            {
                case "AH":
                    Register2index = 0;
                    break;
                case "AL":
                    Register2index = 1;
                    break;
                case "BH":
                    Register2index = 2;
                    break;
                case "BL":
                    Register2index = 3;
                    break;
                case "CH":
                    Register2index = 4;
                    break;
                case "CL":
                    Register2index = 5;
                    break;
                case "DH":
                    Register2index = 6;
                    break;
                case "DL":
                    Register2index = 7;
                    break;
            }
        }
    }
}