# OSFragmentation

<p align="center">
  <img src="https://user-images.githubusercontent.com/89838040/133051606-06223210-23c6-4aae-bafd-9de57f7e60cf.png" width="550" height="350">
</p>

## But du projet

Afin de pouvoir présenter de façon didactique l’écriture et la fragmentation des fichiers sur l’espace disque, il a été décidé de réaliser une application WPF en C# offrant une visualisation de ces mécanismes. Pour mettre en lumière la différence entre plusieurs systèmes de fichiers, il est possible de visualiser la fragmentation, ainsi que la défragmentation des fichiers en NTFS, système de fichiers utilisé par Windows, ainsi que EXT4, principalement destiné aux systèmes basés sur GNU / Linux.
Le but est donc de vulgariser ces différents concepts par l’intermédiaire d’une application aussi visuelle que possible, dont le fonctionnement est expliqué dans ce document.

## Fonctionnement

Le programme permet ainsi à l’utilisateur de choisir le système de fichier dans lequel il désire visualiser l'écriture des fichiers à l’aide d’une liste déroulante. 
Il peut également choisir la taille des fichiers à écrire pour prendre plus ou moins d’espace sur le disque, représenté par des carrés de différentes couleurs générées automatiquement, différente pour chaque fichier. 
Via le bouton « Ajouter des fichiers aléatoires », on peut directement ajouter plusieurs fichiers sur l’espace disque. Une fois que des fichiers sont ajoutés, l’utilisateur dispose de différentes fonctions. Avec le bouton « Lire », le programme va parcourir la mémoire à l’aide d’un curseur et consommer le fichier.
Le bouton «  Défragmenter » lance le processus de défragmentation.  Le curseur parcourt la mémoire et réarrange tous les blocs de fichiers en les regroupant au début de la mémoire.
Une fois les fonctions utilisées, il est possible de nettoyer l’espace disque avec le bouton « Nettoyer » et de faire disparaître ainsi tous les fichiers.

