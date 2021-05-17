using System.Threading.Tasks;
using System.Windows.Controls;

namespace CI.DS.Visual.Views
{
  public partial class AllCashByGroupView : UserControl
  {
    public AllCashByGroupView()
    {
      InitializeComponent();
      Loaded += async (s, e) => { await Task.Delay(99); focus0.Focus(); };
    }
  }
}
