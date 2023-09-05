
public class PotionItem : CountableItem, IUsableItem
{
    public PotionItem(PotionItemData Data, int Amount = 1) : base(Data, Amount) { }

    public override CountableItem Clone()
    {
        return new PotionItem(CountableData as PotionItemData);
    }

    public bool Use() //�� �������� ����ϸ� ����� �Լ�
    {
        Amount -= 1;
        return true;
    }

}
