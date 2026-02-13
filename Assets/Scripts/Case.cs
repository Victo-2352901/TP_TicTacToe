using UnityEngine;

public class Case : MonoBehaviour
{

    [SerializeField]
    public int index;
    

    private bool estOccupee = false;

    /// <summary>
    /// Vérifie si la case est occupée et si non renvoi le traitement à PlacerSymbole.
    /// </summary>
    /// <param name="controlleurJeu">Le script qui controle le jeu</param>
    public void Cliquer(Placement controlleurJeu)
    {
        if (estOccupee) return;

        estOccupee = true;

        controlleurJeu.PlacerSymbole(this);
    }
    
    /// <summary>
    ///     Renvoi si la case est occupée ou non
    /// </summary>
    /// <returns></returns>
    public bool EstOccupee()
    {
        return estOccupee;
    }

    /// <summary>
    /// Met la valeur estOccupee à false pour réinitialiser les cases
    /// </summary>
    public void Reinitialisation()
    {
        estOccupee = false;
    }
}
