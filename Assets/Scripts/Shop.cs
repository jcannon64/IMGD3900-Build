using UnityEngine;

public class Shop : MonoBehaviour
{
    public int itemNum = 0;

    private void OnTriggerStay2D(Collider2D collision) {
        if(Input.GetButton("Submit")) {
            switch(itemNum) {
                case 0:
                    if(Manager.chips >= 50) {
                        FindAnyObjectByType<Player>().GainChips(-50);
                        int cardType = Random.Range(1, 5);
                        if(cardType == 1) {
                            Manager.spadesAmmo += 1;
                        }
                        if(cardType == 2) {
                            Manager.heartsAmmo += 1;
                        }
                        if(cardType == 3) {
                            Manager.clubsAmmo += 1;
                        }
                        if(cardType == 4) {
                            Manager.diamondsAmmo += 1;
                        }
                    }
                    break;
                case 1:
                    if(Manager.chips >= 500 && Manager.spadesUpgrade == false) {
                        Manager.spadesUpgrade = true;
                        FindAnyObjectByType<Player>().GainChips(-500);
                    }
                    break;
                case 2:
                    if(Manager.chips >= 500 && Manager.heartsUpgrade == false) {
                        Manager.heartsUpgrade = true;
                        FindAnyObjectByType<Player>().GainChips(-500);
                    }
                    break;
                case 3:
                    if(Manager.chips >= 500 && Manager.clubsUpgrade == false) {
                        Manager.clubsUpgrade = true;
                        FindAnyObjectByType<Player>().GainChips(-500);
                    }
                    break;
                case 4:
                    if(Manager.chips >= 500 && Manager.diamondsUpgrade == false) {
                        Manager.diamondsUpgrade = true;
                        FindAnyObjectByType<Player>().GainChips(-500);
                    }
                    break;
            }
        }
    }
}
