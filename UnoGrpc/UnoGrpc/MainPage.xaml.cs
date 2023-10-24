using UnoGrpcCommon;

namespace UnoGrpc;

public sealed partial class MainPage : Page {

    public MainPage() {
        this.InitializeComponent();
    }

    private void Compute_Click(object sender, RoutedEventArgs e) {
        if (long.TryParse(this.Op1.Text, out long op1) == false
            || long.TryParse(this.Op2.Text, out long op2) == false) {
            this.Result.Text = "Enter numeric values for operands";
            return;
        }

		try {
            UnoGrpcClient.AddAsync(op1, op2).ContinueWith((task) => {
                var result = task.Result;
                this.Result.Text = result.Message == "OK" ? $"{op1} + {op2} = {result.Result.ToString()}" : result.Message;
            });
		}
		catch (Exception ex ) {
            this.Result.Text = ex.Message;
		}
    }
}
