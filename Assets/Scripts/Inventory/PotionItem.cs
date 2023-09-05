
public class PotionItem : CountableItem, IUsableItem
{
    public PotionItem(PotionItemData Data, int Amount = 1) : base(Data, Amount) { }

    public override CountableItem Clone()
    {
        return new PotionItem(CountableData as PotionItemData);
    }

    public bool Use() //이 아이템을 사용하면 실행될 함수
    {
        Amount -= 1;
        return true;
    }

}
