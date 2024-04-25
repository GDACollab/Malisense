<b>WEEPING CLERIC</b>: I hear shameful footsteps upon the sacred marble floors.
<b>SMILING CLERIC</b>: Of course! The wretched dreamer returns with broken spirits!
<b>THINKING CLERIC</b>: I wonder, will it continue to flail in misguided misery... or has it come here to bathe in the holy light?

<i>The condescension drips sickly sour off of their lips as their eyes alight upon you. The grand cathedral around them seems to mock you, too, as their words  reverberate off the walls. You can almost imagine the sound of a chuckle from the dark throne cloaked in shadow at the opposite end of the vaulted hall.</i>
 * <b>PLAYER</b>: I have no need for holy light, but I could use some answers.
    <i>The thoughtful cleric spreads their arms wide.</i>
    <b>THINKING</b>: We have little to hide. As long as you remain diplomatic, we will answer any questions you possess.
        -> AskQuestions
 * <b>PLAYER</b>: Let me pass, petulant ones. I care not for your distractions.
    -> GoForth

== AskQuestions ==
 * <b>PLAYER</b>: Who are you?
    -> WhoAreYou
 * <b>PLAYER</b>: Why are you still here? 
    -> WhyStillHere
 * <b>PLAYER</b>: The church was supposed to survive the cataclysm. What happened to everyone else? Where is the High Priest?
    -> WhatHappenedToChurch
 * <b>PLAYER</b>: Enough. I  wish to pass on into the dungeon.
    -> GoForth

== WhoAreYou ==
<b>WEEPING</b>: I am Ila, acolyte of repose.
<b>SMILING</b>: I am Ina, acolyte of beast. 
<b>THINKING</b>: I am Ana, acolyte of word. Who are <i>you</i>?
 * <b>PLAYER</b>: I am the endless repose.
    <b>WEEPING</b>: Doubtful...
 * <b>PLAYER</b>: I am the lonely beast.
    <b>SMILING</b>: Laughable!
 * <b>PLAYER</b>: I am the final word.
    <b>THINKING</b>: Intriguing. 
 * <b>PLAYER</b>: I am the disgraced.
    <b>THINKING</b>: Without a doubt. 
- -> AskQuestions

== WhyStillHere ==
<b>WEEPING</b>: 

-> AskQuestions

== WhatHappenedToChurch ==
<b>WEEPING</b>: 

-> AskQuestions

== GoForth ==
<b>SMILING</b>: We will see you again soon, regardless of whether success or failure sinks its fangs into you first.
<b>WEEPING</b>: Though I do not have hope in your ability to escape the cold embrace of death any time soon. Mortality cannot be cheated forever...
    -> END



