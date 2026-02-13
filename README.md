##   Titre du projet   ##
TP_TicTacToe

##  Description ##
Projet Unity d'un jeu de Tic Tac Toe en AR. L'utilisateur place une grille et le jeu commence.
Chacun son tour les deux joueurs place leurs symoles et celui qui obtient trois symboles en ligne ou diagonale gagne.


## Version Unity et Packages ##
La version de Unity utilisé est la 6000.3.5f1

# Packages #
Google ARCore XR Plugin
AR Foundation


## Défis rencontrés ##
J'ai eu de la misère avec le ARAnchor de la grille. J'ai pris les lignes de code provenant des notes de cours mais cela mettait le ARAnchor sur le plan. Cela faisait en sorte que ma grille bougeais quand les plan n'étais pas totalement charger. Je me suis renseigner auprès de mes amis et la solution final est de créer un nouveau GameObject sur lequel on va lui ajouter le ARAnchor à la position du tap.




## Annexe ##
Requête envoyer à ChatGPT : 

Pourrais tu me donner plusieurs moyens pour vérifier si les cases cliqué sont libre et à quelle position ils sont dans mon jeu de tic tac toe.

Voici mon tableau de jeu tic tac toe. (private string[] plateau = new string[9];) Pourrais tu m'expliquer pas à pas comment faire pour vérifier les combinaisons gagnantes.

J'aimerais que les symboles aient des animations pour chaque combinaison pourrais tu prendre mon exemple et remplir les autres combinaisons afin de me sauver du temps.
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



##   Auteur  ##
Anthony Ramsay

