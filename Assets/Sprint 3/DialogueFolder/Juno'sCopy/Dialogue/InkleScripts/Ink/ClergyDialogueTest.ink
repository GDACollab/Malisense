VAR angryClerics = false

<b>WEEPING CLERIC</b>: We face failure! How dare we yet breathe while the Perfect One suffers below…
- <b>SMILING CLERIC</b>: No, brother, we face success! The Perfect One has taken the city and dragged it beneath the lofty reaches of our sacred spire!
- <b>THINKING CLERIC</b>: We may still face either fate, my kin, but be aware, a fallen cleric listens.

- <b>WEEPING</b>: Do you come to beg forgiveness?

- <b>SMILING</b>: Do you come to share our mirth?

- <b>PLAYER</b>: <i>You look between each face, crossing your arms.</i>
 * <b>PLAYER</b>: I seek the artifact. 
    -> ADoomedQuest
 * <b>PLAYER</b>: I seek a setting sun on your god.  
    -> TrulyDisgraced
 * <b>PLAYER</b>: I seek nothing but safe passage below. 
    -> ADoomedQuest

== ADoomedQuest ==
<b>WEEPING</b>: A doomed quest…
* [Next]
- <b>THINKING</b>: …but a useful experiment.
* [Next]
- <b>SMILING</b>: Of course, disgraced one, you may pass into our lord's lair and meet his children. May it rekindle your faith.
* [Next]
-> GoForth

==TrulyDisgraced ==
~ angryClerics = true
<b>WEEPING</b>: Disgraceful, as expected...
* [Next]
- <b>SMILING</b>: And what a grand promise! There will be equally grand humor in your failure.
* [Next]
- <b>THINKING</b>: Poor heretic... we will speak to you no longer. Your empty words waste time which could have been spent in silence.
* [Next]
-> GoForth

== GoForth ==
<b>THINKING</b>: <i>Asitotheh ko’ila pri’on anikoli</i>, may you fear that which possesses powerful senses, foolish child. Now, <i>fi</i>, {angryClerics:begone.|go forth.}
    -> END

