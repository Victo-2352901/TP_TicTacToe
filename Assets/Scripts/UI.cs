using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField]
    private GameObject boutonReinitialiser;

    [SerializeField]
    private GameObject boutonNouvellePartie;

    [SerializeField]
    private TextMeshProUGUI text;

    public static UI instance;


    /// <summary>
    /// Prépare le singleton et les paramètres de base afin de bien faire fonctionner le jeu
    /// </summary>
    void Start()
    {
        boutonReinitialiser.SetActive(false);

        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /// <summary>
    ///     Affiche le bouton de réinitialisation
    /// </summary>
    public void AfficherBoutonReinitialiser()
    {
        boutonReinitialiser.SetActive(true);
    }

    /// <summary>
    ///     Cache le bouton de réinitialisation
    /// </summary>
    public void EnleverBoutonReinitialiser()
    {
        boutonReinitialiser.SetActive(false);
        text.text = "Scanner une surface et positionner la grille";
    }

    /// <summary>
    /// Affiche le tour du joueur en cour
    /// </summary>
    /// <param name="joueurActuel">Je joueur actif</param>
    public void Tour(string joueurActuel)
    {
        text.text = "Tour des : " + joueurActuel;
    }

    /// <summary>
    ///     Affiche le texte de la partie gagnée
    /// </summary>
    /// <param name="joueurActuel">Je joueur actif</param>
    public void MatchGagner(string joueurActuel)
    {
        text.text = "Les " + joueurActuel + " ont gagné !";
    }

    /// <summary>
    ///     Change le texte en haut de l'écran pour  "Le match est nul !"
    /// </summary>
    public void MatchNul()
    {
        text.text = "Le match est nul !";
    }

    /// <summary>
    ///     Change le texte en haut de l'écran pour  "Impossible de placer la grille sur cette surface"
    /// </summary>
    public void PositionIncorrect()
    {
        text.text = "Impossible de placer la grille sur cette surface";
    }

    /// <summary>
    ///     Affiche le bouton de nouvelle partie
    /// </summary>
    public void AfficherNouvellePartie()
    {
        boutonReinitialiser.SetActive(true);
    }

    /// <summary>
    ///     Cache le bouton de nouvelle partie
    /// </summary>
    public void CacherNouvellePartie()
    {
        boutonReinitialiser.SetActive(false);
    }
}
