
public class PotionItem : CountableItem, IUsableItem
{
    public PotionItemData PotionData { get; }
    public PotionItem(PotionItemData Data, int Amount = 1) : base(Data, Amount)
    {
        PotionData = Data;
    }

    public override CountableItem Clone()
    {
        return new PotionItem(CountableData as PotionItemData);
    }

    public bool Use() //이 아이템을 사용하면 실행될 함수
    {
        GameManager.Instance.Player.Inventory.SubItem(this, 1);
        GameManager.Instance.Player.RecoverHp(Data.Name, PotionData.Value);
        UIManager.Instance.ShowCenterText(Data.Name + "를(을) 사용했습니다.");
        return true;
    }

}
