using System.Collections.Generic;
using TMPro;
using Unity.Hierarchy;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.UIElements.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class Placement : MonoBehaviour
{



    [SerializeField] 
    private ARRaycastManager arRaycastManager;


    /// <summary>
    ///     Section pour la grille
    /// </summary>
    
    // Prefabs de la grille
    [SerializeField]
    private GameObject grille;

    // GameObject de la grille placer dans le jeu
    private GameObject grilleActive;

    /// <summary>
    ///     Symbole préfabs joueur 
    /// </summary>

    // Le préfab du signe X
    [SerializeField]
    private GameObject prefabX;

    // Le préfab du signe O
    [SerializeField]
    private GameObject prefabO;


    private List<Case> cases = new List<Case>();

    private GameObject objetAPlacer; 

    private bool jeuEnCour = false;

    private int tourActuel = 0;

    private string joueurActif = "X";

    GameObject anchor;

    [SerializeField]
    private GameObject[] symboles = new GameObject[9];

    private string[] plateau = new string[9];

    private string gagnant = null;
    Animator animatorSymboleActuel = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        objetAPlacer = prefabX;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /// <summary>
    ///     Supprime la grille de l'environnement 3d
    /// </summary>
    private void SupprimerGrille()
    {
        if (grilleActive != null)
        {
            Destroy(grilleActive);
            grilleActive = null;
        }
    }

    /// <summary>
    /// Affiche une grille si aucune n'est présente actuellement a la position du toucher de l'utilisateur
    /// </summary>
    /// <param name="context"></param>
    public void OnTap(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // Prend la position actuelle du tap
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (jeuEnCour == false)
            {
                // Liste pour stocker les résultats du raycast AR
                List<ARRaycastHit> hits = new List<ARRaycastHit>();

                // Lancer un raycast AR vers les plans détectés
                if (arRaycastManager.Raycast(ray, hits, TrackableType.PlaneWithinPolygon))
                {
                    // Prendre le premier plan touché
                    Pose hitPose = hits[0].pose;

                    // Obtenir le plan touché
                    ARPlane plane = hits[0].trackable as ARPlane;

                    if (grilleActive == null)
                    {

                        // Adapter selon le type de plan
                        Vector3 position = hitPose.position;


                        if (plane.alignment == PlaneAlignment.HorizontalUp)
                        {
                            // Sol ou table - position normale
                            position += Vector3.up * 0.10f; // Légèrement au-dessus

                            // Créer la grille à la position du plan
                            grilleActive = Instantiate(this.grille, position, Quaternion.identity);

                            // Code trouver dans le cul d'un ours polaire
                            if (!anchor)
                            {
                                anchor = new GameObject("Anchor");
                                anchor.AddComponent<ARAnchor>();
                                anchor.transform.position = position;
                            }


                            grilleActive.transform.parent = anchor.transform;



                            UI.instance.AfficherBoutonReinitialiser();
                            UI.instance.AfficherNouvellePartie();

                            UI.instance.Tour(joueurActif);

                            jeuEnCour = true;

                            // Assigner le tag pour pouvoir supprimer plus tard
                            grilleActive.tag = "Grille";
                        }
                        else
                        {
                            UI.instance.PositionIncorrect();
                        }
                    }
                }
            }
            else
            {
                if(tourActuel == 0)
                {
                    joueurActif = "X";
                    objetAPlacer = prefabX;
                }
                RaycastHit hit;

                // Lancer le rayon
                if (Physics.Raycast(ray, out hit))
                {
                    List<ARRaycastHit> hits = new List<ARRaycastHit>();

                    Case caseCliquee = hit.collider.GetComponent<Case>();

                    if (caseCliquee != null)
                    {
                        cases.Add(caseCliquee);
                        caseCliquee.Cliquer(this);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Place le symbole du joueur actif dans la case respective. Seulement si la case n'est pas occupé
    /// </summary>
    /// <param name="caseCliquee">La case qui à été cliquée</param>
    public void PlacerSymbole(Case caseCliquee)
    {
        if (plateau[caseCliquee.index] == null)
        {
            if(animatorSymboleActuel){
                ArreterAnimations();
            }

            // Placer le prefab au centre de la case
            GameObject symboleAPlacer = Instantiate(objetAPlacer, caseCliquee.transform.position, Quaternion.identity);

            symboles[caseCliquee.index] = symboleAPlacer;

            plateau[caseCliquee.index] = joueurActif;

            tourActuel += 1;

            animatorSymboleActuel = symboleAPlacer.GetComponent<Animator>();

            animatorSymboleActuel.SetBool("Anim", true);

            ChangerJoueur();



            UI.instance.Tour(joueurActif);

            gagnant = VerifierGagnant();

            if (gagnant != null)
            {
                UI.instance.MatchGagner(gagnant);
                jeuEnCour = false;
            }

            // Match nul
            if (tourActuel == 9 && gagnant == null)
            {
                UI.instance.MatchNul();
                jeuEnCour = false;
            }

        }
    }

    /// <summary>
    ///     Supprime tout les symboles qui ont été jouer sur la grille
    /// </summary>
    private void SupprimerSymboles()
    {
        for (int i = 0; i < symboles.Length; i++)
        {
            Destroy(symboles[i]);
        }

        symboles = new GameObject[9];
    }

    /// <summary>
    ///     S'occupe de tout remettre les paramètres par défaut pour la prochaine partie
    /// </summary>
    public void RecommencerPartie(bool supprimerGrille)
    {
        plateau = new string[9];

        tourActuel = 0;

        joueurActif = "X";
        objetAPlacer = prefabX;


        SupprimerSymboles();

        if (supprimerGrille)
        {
            SupprimerGrille();
            UI.instance.EnleverBoutonReinitialiser();
            UI.instance.CacherNouvellePartie();
            jeuEnCour = false;
        }
        else
        {
            UI.instance.Tour(joueurActif);
            jeuEnCour = true;
            ReinitialiserCases();
        }
    }

    /// <summary>
    /// Remet les cases du tic tac toe libre
    /// </summary>
    private void ReinitialiserCases()
    {
        for (int i = 0; i < cases.Count; i++)
        {
            cases[i].Reinitialisation();
        }
    }
    


    /// <summary>
    ///     Change le joueur actif
    /// </summary>
    private void ChangerJoueur()
    {
        if (joueurActif == "X")
        {
            joueurActif = "O";
            objetAPlacer = prefabO;
        }
        else
        {
            joueurActif = "X";
            objetAPlacer = prefabX;
        }
    }


    /// <summary>
    /// Cette méthode provient de ChatGPT
    /// Vérifie toutes les possibilitées d'avoir gagné
    /// </summary>
    /// <returns>Le gagnant s'il y à lieu. Aussi non NULL</returns>
    private string VerifierGagnant()
    {
        // Lignes
        if (plateau[0] != null && plateau[0] == plateau[1] && plateau[1] == plateau[2])
        {
            Animator animator = symboles[0].GetComponent<Animator>();
            animator.SetBool("Anim", true);

            animator = symboles[1].GetComponent<Animator>();
            animator.SetBool("Anim", true);

            animator = symboles[2].GetComponent<Animator>();
            animator.SetBool("Anim", true);

            return plateau[0];
        }
            
        if (plateau[3] != null && plateau[3] == plateau[4] && plateau[4] == plateau[5])
        {

            Animator animator = symboles[3].GetComponent<Animator>();
            animator.SetBool("Anim", true);

            animator = symboles[4].GetComponent<Animator>();
            animator.SetBool("Anim", true);

            animator = symboles[5].GetComponent<Animator>();
            animator.SetBool("Anim", true);
            return plateau[3];
        }
            

        if (plateau[6] != null && plateau[6] == plateau[7] && plateau[7] == plateau[8])
        {
            Animator animator = symboles[6].GetComponent<Animator>();
            animator.SetBool("Anim", true);

            animator = symboles[7].GetComponent<Animator>();
            animator.SetBool("Anim", true);

            animator = symboles[8].GetComponent<Animator>();
            animator.SetBool("Anim", true);
            return plateau[6];
        }


        // Colonnes
        if (plateau[0] != null && plateau[0] == plateau[3] && plateau[3] == plateau[6])
        {
            Animator animator = symboles[0].GetComponent<Animator>();
            animator.SetBool("Anim", true);

            animator = symboles[3].GetComponent<Animator>();
            animator.SetBool("Anim", true);

            animator = symboles[6].GetComponent<Animator>();
            animator.SetBool("Anim", true);
            return plateau[0];
        }


        if (plateau[1] != null && plateau[1] == plateau[4] && plateau[4] == plateau[7])
        {
            Animator animator = symboles[1].GetComponent<Animator>();
            animator.SetBool("Anim", true);

            animator = symboles[4].GetComponent<Animator>();
            animator.SetBool("Anim", true);

            animator = symboles[7].GetComponent<Animator>();
            animator.SetBool("Anim", true);
            return plateau[1];
        }

        if (plateau[2] != null && plateau[2] == plateau[5] && plateau[5] == plateau[8])
        {

            Animator animator = symboles[2].GetComponent<Animator>();
            animator.SetBool("Anim", true);

            animator = symboles[5].GetComponent<Animator>();
            animator.SetBool("Anim", true);

            animator = symboles[8].GetComponent<Animator>();
            animator.SetBool("Anim", true);
            return plateau[2];
        }


        // Diagonales
        if (plateau[0] != null && plateau[0] == plateau[4] && plateau[4] == plateau[8])
        {
            Animator animator = symboles[0].GetComponent<Animator>();
            animator.SetBool("Anim", true);

            animator = symboles[4].GetComponent<Animator>();
            animator.SetBool("Anim", true);

            animator = symboles[8].GetComponent<Animator>();
            animator.SetBool("Anim", true);
            return plateau[0];
        }


        if (plateau[2] != null && plateau[2] == plateau[4] && plateau[4] == plateau[6])
        {
            Animator animator = symboles[2].GetComponent<Animator>();
            animator.SetBool("Anim", true);

            animator = symboles[4].GetComponent<Animator>();
            animator.SetBool("Anim", true);

            animator = symboles[6].GetComponent<Animator>();
            animator.SetBool("Anim", true);
            return plateau[2];
        }


        return null;
    }

    /// <summary>
    ///     Arrete tout les animations jouer par les symboles
    /// </summary>
    private void ArreterAnimations()
    {
        for (int i = 0; i < symboles.Length; i++)
        {
                Animator animator = symboles[i].GetComponent<Animator>();
                if (animator != null)
                {
                    animator.SetBool("Anim", false);
                }
        }
    }
}
