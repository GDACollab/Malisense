VAR isIntro = false
VAR isDeathF1 = false
VAR isHub = true
VAR isDeathF2 = false 
VAR isIntroductionCutscene = false
VAR isEnd = false
VAR hasDied = false

VAR isScholarIntro = false
VAR isMayorIntro = false

VAR highPriestWasIntroduced = true
VAR toldCKAboutHighPriest = false

VAR hasMayorNote1 = false
VAR hasMayorNote2 = false
VAR hasFinalMayorNote = false

VAR background = "First"
VAR StickHappiness = 0

/*
Possible Characters:
Crypt Keeper 
Clergy 
Stick 
Mayor 
Scholar 
*/

// Keep this variable updated based on current character:
VAR CharacterTitle = "????????????"

VAR character = "Crypt_Keeper"

-> START

== START ==
{
 - isEnd: -> End
 - isIntroductionCutscene: -> Introduction // Go to Introduction
 - character == "Crypt_Keeper": -> Crypt_Keeper // Go to CK 
 - character == "Stick": -> Stick // Go to Stick
 - character == "Mayor": -> Mayor // Go to Mayor
 - character == "Clergy": -> Clergy // Go to Clergy
 - character == "Scholar": -> Scholar // Go to Scholar 
 - else: Error -> END
 }
 


/*
Sections:
Introduction
NPCs x5
End

Subsections:
bool Variables
*/
 
 == Introduction ==
~ CharacterTitle = ""
<i>Everything is coated in darkness. </i>

<i>It reeks of death.</i>

<i>Your mind flashes... </i>

<i>You hear screams...</i>

<i>Flickers of blood...</i>

<i>Sight, hearing, touch, taste, scent...restored but not recovered. </i>

<i>You blink into consciousness, yet the world around you does not.</i>

<i>The memories fade, but the smell of death still lingers.</i>

<i>Your soul hangs in between, familiar but foreign.</i>

<i>But, everything happens for a reason.</i>

<i>Right?</i>

-> END
 

/* PreF1 Reserved for Crypt Keeper and Clergy. If you are working on other characters, please delete this section */
==Crypt_Keeper==
~ CharacterTitle = "Crypt Keeper"
{
 - isIntro: -> intro // Go to intro
 - isDeathF1: -> DeathF1 // Go to death 1
 - isHub: -> hub // Go to hub
 - isDeathF2: -> DeathF2 // Go to death 2 
 - isEnd: -> End // Go to end
 - else: Error ->END
 }

 =intro
 {intro > 1: -> CKIntroRevisit}
~ CharacterTitle = ""
<i>You approach the familiar figure. Your throat tightens as she turns to you. </i>

<i>Her warm smile softens the gloom of the decaying fountain. </i>
~ CharacterTitle = "????????????"
  "I don’t need my sight to recognize your presence, little sweet. It has been a long while since anyone has dared venture to this old place." 
 
   "It's rude to keep a lady waiting...even one with all the time in the world." 
~ CharacterTitle = ""
  <i>She chuckles softly.</i>

 * ["Who are you...again?"]
 ~ CharacterTitle = "????????????"
    "You don't recall? Perhaps that is to be expected, considering your state."
~ CharacterTitle = ""
    <i>She lets out a quiet sigh. </i>
    ~ CharacterTitle = "????????????"
    "I'm sure you'll remember soon enough. Let's just say you're quite special to me, and I'm quite special to you." -> whathappened
   
 * ["Hello?"]
 ~ CharacterTitle = "????????????"
    "Oh, thank the heavens. I was so worried that, in all the tumult, you might have forgotten who I was."
    
    "But I guess that's how peonies are–just when you think they've wilted completely, they suddenly blossom brighter than ever before."
   ~ CharacterTitle = ""
   <i>You feel as if you know this person, but struggle to even recall her name.</i> -> whathappened
   
   =whathappened
   *["What happened?"]
   ~ CharacterTitle = "????????????"
   "Hmm...I guess there is no easy way to say this." 
   ~ CharacterTitle = ""
   <i>The woman's face becomes pale. She fidgets with her necklace. </i>
   ~ CharacterTitle = "????????????"
   "You...passed on. Oh, you can only imagine how distraught I was to find your lifeless body out here. Thankfully, I was able to collect myself and begin preparations to bring you back." 
  ~ CharacterTitle = "Crypt Keeper"
  I am the keeper of this crypt, after all. It took a fair bit of work, yes, but you know I'd do anything for my peony." -> whathappenedcont
    
   =whathappenedcont
    
    *["How did you do such a thing?"]
     ~ CharacterTitle = "Crypt Keeper"
   "A magician never reveals her secrets...but since you're special, I'll make an exception." 
   
    "That lantern that hangs from your staff contains your soul. When in close proximity with it, your spirit can once again animate your body as it did while you were still...living."
   
    "I even carefully fitted your latern with some precious stones. If you're ever in danger, those gems will bring you back to me. I'm always here to take care of you, darling. When you feel the latern's glow, think of it as my embrace."
    -> lanterndesc
   
   =lanterndesc
   ~ CharacterTitle = ""
    <i>You look down at the lantern.</i>
     
    <i>Within it, a small candle burns soflty. The flicker of its flame is comforting. </i>
     
    <i>It reminds you of the broth you drank as a child when you were ill. </i>
     
    <i>The lantern itself is ornately sculpted. It's so polished that you can see yourself reflected in its silver casing. </i>
     
    <i>Someone clearly has put a lot of effort into preparing it for this occassion.</i> -> interrogation
     
    =interrogation
    *["Where is everyone?"]
    ~ CharacterTitle = "Crypt Keeper"
        "You don't remember? I'll tell you this much--I was right about that creature."
        
       "Radefell is gone. The survivors are living in this village. I've been helping them, here and there." 
        
        "I bet they'll be quite happy to see you. But please, let's not discuss this any further." 
        
        "Besides, you're already in such a sorry state. I can't imagine that dwelling on bad news is making you feel any better." -> interrogation
   
    *["Where am I?"]
    ~ CharacterTitle = "Crypt Keeper"
        "Apologies for the drab scenery. It's much easier to bring a soul back in a place such as this." 
        
        "Resuscitation is difficult in a city, where death is so omnnipresent."
        
        "But I guess it's an apt setting, seeing as I've cleaned you up like the mother dove cleans her young in the fountain's basin." -> interrogation
        
    *["Why revive me?"]
      ~ CharacterTitle = ""
      <i>She frowns. </i>
        ~ CharacterTitle = "Crypt Keeper"
        "Why revive anyone else? You're the lantern of my life, peony. You should know that better than anyone else." -> interrogation
    
    *["How did I die?"]
      ~ CharacterTitle = "Crypt Keeper"
        "I found you not far from here, in a nearby cornfield. You were badly bruised, especially on your knees and elbows."
       
       "It was a terrible scene, just awful. I was quite distraught, but I managed to pull myself together."
       
      "'This won't do.', I told myself, 'This won't do.' So I picked you up--all of you, including the fingers you had lost--and put you back together." 
      
      "Don't worry, I didn't peek under your mask. I know you're awfully sensitive about that."  -> interrogation
       

    *{CHOICE_COUNT() == 0}-> 
    ~ CharacterTitle = "Crypt Keeper"
       "Now then, I'm afraid I must wish you farewell. But worry not, my darling, for we shall meet again very soon." 
       ~ CharacterTitle = ""
       <i>She smiles at you, and–for the first time in a long time–you feel safe. </i>
       
       <i>You were safe at the chapel, sure, but any bird is safe in a cage. This is different. </i>
       
       <i>The woman snaps her fingers and disappears, leaving nothing behind but a faint aroma of lavender.</i>

        -> END

= CKIntroRevisit
~ CharacterTitle = ""
<i>The soft scent of flowers floats on the wind.</i>
     
<i>All is deathly quiet.</i>
-> END

= DeathF1
{DeathF1 > 1: -> CKBedrock}
~ CharacterTitle = ""
<i>In an instant, you feel yourself being thrust back into the cold embrace of life.</i>

~ CharacterTitle = "Crypt Keeper"
"You’ve met with yet another terrible end, haven’t you?" 

"Not to worry. I’ll be here–as I always have been–to put your soul back in its place." 

"Just...promise me you won’t do something I can’t bring you back from." 

"Don’t let your hunger for redemption destroy what we still have."

"Breathe."
-> END

 =hub
{hub > 1: -> CKBedrock}
~ CharacterTitle = ""
{highPriestWasIntroduced && (toldCKAboutHighPriest != true): <i>You return to the Crypt Keeper, finding her tending to the crypt’s flora.</i> -> Artifact1}
~ CharacterTitle = "Crypt Keeper"
"Hello, dear. I'd love to talk–that is, if you don't have any pressing matters to attend to."
~ CharacterTitle = ""

*["I have a question."]
~ CharacterTitle = "Crypt Keeper"
"What would you like to discuss, my peony?" -> conversation
*["Goodbye."]
~ CharacterTitle = "Crypt Keeper"
"Until next time, then. Don't keep me waiting." -> END
 
=conversation
~ CharacterTitle = ""
+["Why do we need the artifacts?"]
~ CharacterTitle = "Crypt Keeper"
"Well, that wretched being wants the artifacts."

"Thus, we have two choices..."

"We can let it carry out its scheme..."

"or we can intervene."

"That creature already destroyed Radefell. We can't let it move any further towards its goal." -> conversation

+["What exactly are the dungeons?"]
~ CharacterTitle = "Crypt Keeper"
"When the Malignance emerged, it wrought havoc upon Radefell. But it wasn't just blindly destroying everything." 

"It was taking pieces of the city and pulling them underground. This created the dungeons you now traverse."

"As to why would it do such a thing? I'm not sure, but I reckon that we'll find out soon enough." -> conversation

+["How are you holding up?"]
~ CharacterTitle = "Crypt Keeper"
"How kind of you to worry about me. I'm doing well, all things considered."

"In a situation like this, one is bound to miss her creature comforts." 

"I miss my bed, and the food here is...of variable quality." 

"But I still have you, my peony. That's all I need." -> conversation

+["Goodbye."]
~ CharacterTitle = "Crypt Keeper"
 "Until next time, then. Don't keep me waiting." -> END

= DeathF2
{DeathF2 > 1: -> hub}
~ CharacterTitle = "Crypt Keeper"
{ shuffle once:
-   "How disappointing..."
- 	"What a thrilling death. I can see you've had practice."
-   "Oh dear, you know how much it pains me to see you get hurt."
-   "Someone's been busy! Let's get you patched up."
-   "I wonder...is it possible for one to get accustomed to death?"
}
~ CharacterTitle = ""
* [<i>Breathe.</i>]
- -> END
=Artifact1
~ CharacterTitle = ""
<i>She seems to not acknowledge your presence. </i>

<i>Have you done something wrong? </i>

<i>Does she know about the High Priest returning? </i>

<i>There's only one way to find out.</i>

+[<i>Wait for her to acknowledge you</i>]-> wait
+[<i>Approach her</i>]-> approach

=wait 
~ CharacterTitle = ""
<i>You observe her care for the plants for minutes on end.</i>
+[<i>Continue to wait</i>]-> continueWait 
+[<i>Approach her</i>]-> approach 

=continueWait
~ CharacterTitle = ""
<i>You stand idly for another minute, even though she has stopped watering the plants. </i>
<i>After the minute you hear her say...</i>
~ CharacterTitle = "Crypt Keeper"
"Well?"
-> approach

=approach
~ CharacterTitle = ""
<i>You take a single step forward.</i>
<i>She chuckles to herself.</i>
~ CharacterTitle = "Crypt Keeper"
"Welcome back, my little sweet. Please, come here." 

<i>Doing as asked, you approach her observing the plant life she cares for. </i>
~ CharacterTitle = "Crypt Keeper"
"Beautiful, isn't it?" 

"Now, be a doll and help me up, would you?" 
~ CharacterTitle = ""
*[<i>Help her up</i>]

<i>She dusts herself off and turns to face you. Not a speck of dirt, dust, or plant remains on her white dress. </i>

<i>She embraces you. </i>
~ CharacterTitle = "Crypt Keeper"
"It all feels so surreal, I knew I trusted in you for the right reason." 

<i>She releases you from her embrace.</i>
~ CharacterTitle = "Crypt Keeper"
"Now, you have the artifact?"
~ CharacterTitle = ""
**[Give her the artifact]

<i>You hand her the bell, and she stares into the faces etched along its surface. </i>
~ CharacterTitle = "Crypt Keeper"
"The Whispering Bell..." 
~ CharacterTitle = ""
<i>She looks back up at you. </i>
~ CharacterTitle = "Crypt Keeper"
"You really did it." 
~ CharacterTitle = ""
<i>The light you saw when you were brought back shines in front of you once again as the Crypt Keeper cleanses the Whispering Bell. </i>

<i>The bell emits pained groans as she does, until their cries of anguish lessen.</i>

<i>Then silence...</i>
~ CharacterTitle = "Crypt Keeper"
"Continue to bring these, and we can clense this world of the Malignance's curse." 

"So... how are you feeling?" 
~ CharacterTitle = ""
<i>You focus is drawn away from the bell and back to the Crypt Keeper as she extends it back towards you to take.</i>

+++["I'm ready to face the next challenge."]-> ready
+++["Perhaps some rest is in order"]-> rest
+++["..."]-> stare 

= ready 
~ CharacterTitle = "Crypt Keeper"
 "Always so eager, I suppose now is no time for a break." -> stare 
 
 = rest
 ~ CharacterTitle = "Crypt Keeper"
  "Of course, a break is always necessary. Just make your return sooner rather than later."
 -> stare
 
 = stare 
 ~ CharacterTitle = ""
<i>She stares at you intensely, expectantly. </i>
~ CharacterTitle = "Crypt Keeper"
 "Is something bothering you?" 
 -> ClergyQuestions 

=ClergyQuestions
~ CharacterTitle = ""
*["Do you feel different at all?"]-> different 
*["Do you know how the High Priest died?"]-> died 
*["What do you know of the three clerics?"]-> threeClerics 
*-> HighPriestRise

= different
~ CharacterTitle = ""
<i>She looks over herself for a moment. </i>
~ CharacterTitle = "Crypt Keeper"
"As far as physically, I don't feel too different." 

"I suspected the moment you secured the artifact. The crypt shook violently, and several of the plants wilted." 

"That's why you found me caring for the ones that are left, knowing that they'll most likely pass on as you secure more of the relics." 

-> ClergyQuestions

= died 
~ CharacterTitle = "Crypt Keeper"
"It is not knowledge that I am aware of."
"I've disconnected myself from my ties with the church. I'd imagine that even if I was still involved with them, I wouldn't get the whole truth. That is simply their nature."
-> ClergyQuestions

= threeClerics 
~ CharacterTitle = ""
<i>She rolls her eyes. </i>
~ CharacterTitle = "Crypt Keeper"
"Yes, I'm acquainted with the three. Nothing more than lackey clerics to the High Priest, but I suppose why not be?"
"It allows yourself an opportunity to surround the High Priest, and in turn some feeling of power."
-> ClergyQuestions

=HighPriestRise
~ CharacterTitle = "Crypt Keeper"
"You still seem off, peony." 
"You can tell me what bothers you, I will always be here for you."
~ CharacterTitle = ""
*["I witnessed the High Priest return from death."]

<i>The Crypt Keeper goes pale, and she finds herself at a loss for words.</i>
~ CharacterTitle = ""
**["Are you ok?"]
~ CharacterTitle = "Crypt Keeper"
"Y-yes... I feared you would eventually bring me news like this, but I thank that you did." 
~ CharacterTitle = ""
<i>She pauses for a minute.</i>
~ CharacterTitle = "Crypt Keeper"
"Please don't worry about me, peony."

"You have a tall task being asked of you, the last thing I'd want to do is distract you from your quest." 

"We may talk about this more at a later date, but I think the best thing for me now is to collect my own thoughts."
~ CharacterTitle = ""
+++[<i>Leave</i>]
    -> leave
+++[<i>Embrace her once more</i>]
    -> embrace 

=embrace 
~ CharacterTitle = ""
<i>You move in to embrace the Crypt Keeper once more. </i>

<i>She wraps her arms around you, tighter than the last. </i>

<i>Once again, she lets go, giving you a wave and a smile.</i>

-> END

=leave 
-> END

=CKBedrock
~ CharacterTitle = "Crypt Keeper"
"Hello my sweet, you have returned once again. It seems as though the dungeons have been rather troublesome."  

 + ["I am not having trouble"]
  ~ CharacterTitle = "Crypt Keeper"
  "I suppose so..." 
  "Yet, I have manifested your revivification more than usual.
  ~ CharacterTitle = ""
 <i>She laughs lightly to herself</i>
 ~ CharacterTitle = "Crypt Keeper"
 "Through my external perspective, it's easy to conclude that you may be having <i>some</i> trouble." 
 -> FinalBedrock
 
 + ["It has been difficult"]
 ~ CharacterTitle = "Crypt Keeper"
  "I had concluded so. I have manifested your revivification more than usual."
   ~ CharacterTitle = ""
 <i> She laughs lightly to herself</i>
 -> FinalBedrock
 
 =FinalBedrock
  ~ CharacterTitle = "Crypt Keeper"
  "Please tread carefully, peony. I await for you here." 
    -> END


==Stick==
~ CharacterTitle = "Stick"
{
 - isHub: -> Hub // Go to hub
 - isDeathF2: -> Hub // Go to death 2 
 - else: Error -> END
 }
 
= Hub 
{StickHappiness:
 - 1: -> Stick1
 - 2: -> Stick2
 - 3: -> Stick3
 - 4: -> Stick4
 - 5: -> Stick4
 - -1: -> StickSad1
 - -2: -> StickSad2
 - -3: -> StickOHNO
 - -10: -> StickOHNO
 - else: -> NeutralStick
 }
 
// = DeathF2
// {
//  - FirstStickDeathF2 && StickHappiness>0: -> ConcernedStick
//  - else: -> Hub
//  }
// -> END

=NeutralStick
~ CharacterTitle = ""
<i>As you enter the Custodian's house, a melancholy dog approaches you.</i>
<i>Its collar reads: "Stick, my loyal and beloved pet"</i>
~ CharacterTitle = ""
<i>Stick whimpers.</i>
 *[<i>Pet Stick</i>]
    ~ StickHappiness = StickHappiness + 1
    ~ CharacterTitle = ""
    <i>You softly pet Stick's head</i>
    ~ CharacterTitle = "Stick"
    Ruff, ruff!
    ~ CharacterTitle = ""
    <i>Stick happily wags its tail as you gently wave goodbye.</i>
    -> DONE
 *[<i>Leave Stick Alone</i>]
    ~ StickHappiness = StickHappiness - 1
    ~ CharacterTitle = ""
    <i>As you turn and leave you can hear Stick whimpering softly.</i>

-> END

=Stick1
~ CharacterTitle = ""
<i>Stick wags its tail happily as you enter.</i>
+[<i>Pet Stick again</i>]
    {StickHappiness<2: 
        ~StickHappiness=StickHappiness+1
    }
    ~ CharacterTitle = ""
    <i>You softly pat Stick's head.</i>
    ~ CharacterTitle = "Stick"
    Ruff, ruff!
    ~ CharacterTitle = ""
    <i>Stick happily wags its tail.</i>
    -> PetStick
 *[<i>Leave</i>]
    ~ StickHappiness = StickHappiness - 1
    ~ CharacterTitle = ""
    <i>You ignore Stick and leave the house.</i>
-> END

=PetStick
 +[<i>Pet Stick again</i>]
 ~ CharacterTitle = ""
    <i>You softly pat Stick's head.</i>
    ~ CharacterTitle = "Stick"
    Ruff, ruff!
    ~ CharacterTitle = ""
    <i>Stick happily wags its tail.</i>
    -> PetStick
 *[<i>Wave goodbye</i>]
 ~ CharacterTitle = ""
    <i>You wave goodbye to Stick as you leave the house.</i>
-> END

=Stick2
~ CharacterTitle = ""
<i>You enter the Custodian's house. Little trinkets jingle on the wall.</i>
<i>At your presence, Stick perks up and its tail starts moving rapidly.</i>
 +[<i>Pet Stick again</i>]
    {StickHappiness<3:
        ~StickHappiness=StickHappiness+1
    }
    ~ CharacterTitle = ""
    <i>You softly rub Stick's head.</i>
    ~ CharacterTitle = "Stick"
    Arf!
    ~ CharacterTitle = ""
    <i>Stick nuzzles into your hand.</i>
    -> PetStick2
 *[<i>Leave</i>]
    ~ StickHappiness = StickHappiness - 1
    ~ CharacterTitle = ""
    <i>You ignore Stick and leave the house.</i>
-> END
    
=PetStick2
+[<i>Keep petting Stick</i>]
    <i>You softly rub Stick's head.</i>
    ~ CharacterTitle = "Stick"
    Arf!
    ~ CharacterTitle = ""
    <i>Stick nuzzles into your hand.</i>
    -> PetStick
 *[<i>Wave goodbye</i>]
 ~ CharacterTitle = ""
    <i>You wave goodbye to Stick as you leave the house.</i>
-> END

=Stick3
~ CharacterTitle = ""
<i>As you enter the Custodian's house, you see Stick get up and walk towards you, its tail swinging rapidly.</i>
~ CharacterTitle = "Stick"
Ruff!
~ CharacterTitle = ""
<i>A bark of triumph.</i>
 *[<i>Pet Stick</i>]
    {StickHappiness<4:
        ~StickHappiness=StickHappiness+1
    }
    ~ CharacterTitle = ""
    <i>You pat stick on the back.</i>
    ~ CharacterTitle = "Stick"
    Woof!
    **["Good dog!"]
    ~ CharacterTitle = "Stick"
        Woof! Woof!
    **[<i>Scratch Stick's back some more</i>]
    ~ CharacterTitle = "Stick"
        Arf!
    - ~ CharacterTitle = ""
    <i>Stick's tail vigorously wags back and forth.</i>
    <i>You give Stick one last pet and unwillingly leave.</i>
    ~ CharacterTitle = "Stick"
    Bark!
    -> DONE
 *[<i>Leave]
    ~ StickHappiness = StickHappiness - 1
    ~ CharacterTitle = "Stick"
    Ruff! Ruff!
    ~ CharacterTitle = ""
    <i>You immediately turn around and leave, ignoring Stick's barks.</i>
-> END

=Stick4
~ CharacterTitle = ""
<i>As soon as you open the door, Stick launches toward you.</i>
<i>You crouch down and recieve Stick in your arms.</i>
 *[<i>Pet Stick</i>]
    {StickHappiness<5: 
        ~StickHappiness=StickHappiness+1
    }
    ~ CharacterTitle = ""
    <i>You give Stick a hearty back rub.</i>
    ~ CharacterTitle = "Stick"
    Woof! Woof!
    -> PetStick3
*[<i>Get up to leave</i>]
~ CharacterTitle = ""
    <i>As you get up and start to leave, Stick follows.</i>
    -> HappyLeave

=PetStick3
 +[<i>Pet Stick again</i>]
 ~ CharacterTitle = ""
    <i>You give Stick the best belly rub you've ever done in your life.</i>
    ~ CharacterTitle = "Stick"
    Awooo!
    -> PetStick3
 +[<i>Give Stick a treat</i>]
 ~ CharacterTitle = ""
    <i>You find a jar of dog treats on a nearby shelf.</i>
    <i>You give Stick one of the treats.</i>
    ~ CharacterTitle = "Stick"
    Ruff!
    ~ CharacterTitle = ""
    <i>Stick happily gobbles down the treat. </i>
    -> PetStick3
 +[<i>Get Stick to do a trick</i>]
    -> StickTrick
 *[<i>Get up to leave</i>]
 ~ CharacterTitle = ""
    <i>As you get up and start to leave, Stick follows.</i>
    -> HappyLeave
 
 =StickTrick
 VAR trick = 0
 VAR trick2 = 0
 ~ trick = "{~1|2|3|4}"
 ~ trick2 = "{~1|2|3|4|5|6|7|8|9}"
 {trick:
    - 1: 
        ~ CharacterTitle = ""
        <i>You grab a treat and spin it around Stick.</i>
        <i>Stick starts spinning in circles until it gets dizzy and has trouble standing up.</i>
        <i>You give Stick the treat.</i>
        ~ CharacterTitle = "Stick"
        Ruh... Ruff...
    - 2:
        ~ CharacterTitle = ""
        <i>You grab a treat and tell Stick to sit.</i>
        <i>Stick sits down.</i>
        <i>You applaud and give Stick the treat. </i>
        ~ CharacterTitle = "Stick"
        Ruff!
    - 3: 
        ~ CharacterTitle = ""
        <i>You grab a treat and ask Stick to {trick2==1: do a barrel roll|roll over}. </i>
        <i>Stick ferociously rolls over and into the wall. </i>
        ~ CharacterTitle = "Stick"
        Woof! Ruff!
        ~ CharacterTitle = ""
        <i>Stick feels proud of their accomplishment.</i>
        <i>You feed stick the treat.</i>
        ~ CharacterTitle = "Stick"
        Arf!
    - 4: 
        ~ CharacterTitle = ""
        <i>As you go to grab a treat, you witness something amazing.</i>
        <i>Stick does a double flip into a 360 midair spin.</i>
        <i>Stick proudly lands on the floor and you give two treats for that.</i>
        ~ CharacterTitle = "Stick"
        Arf! Arf!
    - else: ERROR RANDOM MODULE FAILED CONTACT YOUR NEAREST PELICAN
}
-> PetStick3

=HappyLeave
~ CharacterTitle = ""
<i>However, the dungeon is too dangerous for a dog, so you stop in your tracks.</i>
 * [<i>Tell Stick to stay</i>]
 ~ CharacterTitle = ""
    <i>You tell Stick to stay.</i>
    ~ CharacterTitle = "Stick"
    *whine*
    ~ CharacterTitle = ""
    <i>Stick whines a bit but understands and backs off.</i>
    <i>With a heavy heart, you leave the Custodian's house.</i>
 * [<i>Try to leave</i>]
 ~ CharacterTitle = ""
    <i>You try to leave, but Stick tries to leave with you.</i>
    -> HappyLeave
 --> END
-> END

=StickSad1
~ CharacterTitle = ""
<i>As you enter the Custodian's house, a heartbroken dog approaches you.</i>
<i>Its collar reads: "Stick, my loyal and beloved pet"</i>
~ CharacterTitle = ""
<i>Stick whimpers.</i>
 * [<i>Pet Stick</i>]
    ~ StickHappiness = StickHappiness + 1
    ~ CharacterTitle = ""
    <i>You softly pet Stick's head.</i>
    ~ CharacterTitle = "Stick"
    Ruff, ruff!
    ~ CharacterTitle = ""
    <i>Stick happily wags its tail as you gently wave goodbye.</i>
    -> DONE
 * [<i>Leave Stick Alone</i>]
 ~ CharacterTitle = ""
    <i>You once again turn and leave, ignoring the whimpers behind you.</i>
-> END

=StickSad2
~ CharacterTitle = ""
<i>You enter the Custodian's house looking around at the various shelves filled with dust.</i>
<i>Stick lays on the ground unresponsive to your intrusion.</i>
    * [<i>Pet Stick</i>]
    ~ CharacterTitle = ""
        <i>You bend down and softly pet Stick's head.</i>
        <i>Stick remains unresponsive to you.</i>
        ** [<i>Keep petting</i>]
            ~ StickHappiness = StickHappiness + 1
            ~ CharacterTitle = ""
            <i>You keep petting Stick.</i>
            <i>Eventually Stick perks up.</i>
            ~ CharacterTitle = "Stick"
            Ruff...
            ~ CharacterTitle = ""
            <i>Stick slightly wags its tail as you gently wave goodbye.</i>
            -> END
        ** [<i>Leave Stick Alone</i>]
        ~ CharacterTitle = ""
            <i>You leave, ignoring Stick alone on the floor.</i>
            -> END
    * [<i>Leave the Custodian's House</i>]
    ~ CharacterTitle = ""
        <i>You leave, ignoring Stick alone on the floor.</i>
        -> END
-> END

=StickOHNO
~ CharacterTitle = ""
<i>You enter the Custodian's house with a creak. It's a dusty room with various trinkets and baubles.</i>
<i>You look around and find a dog bowl, several weird masks, and a few keys. However, you find nothing of importance here. </i>
-> WishfulThinking(StickHappiness)

=WishfulThinking(tries)
 +[<i>Keep looking</i>]
 ~ CharacterTitle = ""
    <i>You keep looking, but you don't find anything of interest.</i>
    -> WishfulThinking(tries-1)
 *{tries == -8}[<i>Sit in silence</i>]
    <i>...</i>
    <i>...</i>
    <i>...</i>
    **[<i>...</i>]
        <i>......</i>
    **[<i>...</i>]
        <i>...............</i>
    
    -- ~ StickHappiness = -10
    -> WishfulThinking(tries-1)
 *[<i>Leave</i>]
 ~ CharacterTitle = ""
    <i>You leave the Custodian's house, wondering why you ever even entered.</i>
-> END

-> END 
==Mayor==
~ CharacterTitle = "Mayor"
{
 - isMayorIntro: -> intro// Go to intro
 - isHub or isDeathF2: -> Hub// Go to hub
 - else: Error
 }
 //INTRO CUTSCENE WHEN HE'S INTRODUCED OUTSIDE OF FLOOR 2
 
 =intro
 {intro > 1: -> MayorBedrock}
 ~ CharacterTitle = ""
<i>As you arise from the dungeon, you notice a familiar man in a top hat, with rosy cheeks and thin, stitched lips. Golden strings hang limply from his limbs, like a discarded marionette. His smile brims with a melancholic mirth.</i>
~ CharacterTitle = "Mayor"
“...What a bitter state of affairs we find ourselves in."

+["You were there."]-> YouWereThere
+["I am sorry."]-> Sorry

=YouWereThere
~ CharacterTitle = ""
<i>The familiar man gives a tasteless, empty chuckle.</i>
~ CharacterTitle = "Mayor"
“Regrettably so, I fear. It brings some small joy that your memory hasn’t rotted like your form.”
“Still, I doubt I am the only one who harbors such remorse.”
-> questions

=Sorry 
~ CharacterTitle = ""
<i>The familiar man breathes deep through his nose, unbothered by the scent of death.</i>
~ CharacterTitle = "Mayor"
“You are not alone in fault, friend. Far from it.”
“Fear and complacency go hand in hand, after all.”

-> questions


= questions 
~ CharacterTitle = ""
{questions < 1:  <i>Recognition finally dredges memory of this man to the surface, one you had never spoken to but had seen so many times: from before the Convergence, the Mayor of Radefell, Poppet Meitar.</i>}
~ CharacterTitle = "Mayor"
“You must have questions. Ask away.”

+["How did you survive?"]-> survive 
+["What happened to the rest of the church?"]-> church
+["What are you doing here?"]-> here 
+["That's all for now."]-> EndOfIntro

=survive
~ CharacterTitle = "Mayor"
“...I fled.”
~ CharacterTitle = ""
<i>Forlorn, the man’s posture visibly sinks.</i>
~ CharacterTitle = "Mayor"
“I hold no deeper shame. I knew that...thing...that something was coming, and I turned my back on those I purported service to.”

“Atonement may never grace me for such deep cowardice.”

-> questions

=church 
~ CharacterTitle = "Mayor"
{church < 1: “...Most are gone. Just like everything else.”}

{church < 1: “Perhaps the only just perdition to have come from the Convergence after all this time.” }
*["Do you know who I am?"]-> whoIam
*["Couldn’t you have stopped what happened?"]-> stopped 
*-> questions

= here 
~ CharacterTitle = "Mayor"
“Much has come about since the Convergence.” 

“By some twist of fate, the town has entrusted me with leading them once more. It remains to be seen if I deserve such station...”
~ CharacterTitle = ""
<i>The Mayor directs a heavy glance through a nearby window.</i>

“...or if there is any good that remains to be done.”
-> questions


=whoIam
~ CharacterTitle = "Mayor"
“Yes, I do recognize you. You were quite the illustrious disciple then...”
~ CharacterTitle = ""
<i>After a moment of silence, the man leans in to regard your appearance in greater detail.</i>
~ CharacterTitle = "Mayor"
“Not many within our strain of history return to this sense-dulled place.”
-> church

=stopped 
~ CharacterTitle = "Mayor"
”I do not know, perhaps there were courses of action.”
 
“The church exploited my station, leaving me nothing but a puppet to their dissolute cause. I only came to the truth until it was too late.“
~ CharacterTitle = ""
<i>A deep sigh escapes from the man, his lips packed into a timeless grimace.</i>
~ CharacterTitle = "Mayor"
“Still, those courses are unexplored, those actions untaken.”

-> church


= EndOfIntro 
~ CharacterTitle = "Mayor"
“If you wish to speak to me further, you can find me in my home.”
~ CharacterTitle = ""
<i>The Mayor regards your appearance and posture. He nods with approval, and speaks as he turns away.</i>
~ CharacterTitle = "Mayor"
“You always possessed an acute poise. Perhaps such a trait will allow you to right these wrongs of ours.”
-> END

=Hub
{Hub > 1: -> MayorBedrock}
~ CharacterTitle = ""
<i>As you enter the room, the Mayor looks to regard you.</i>
~ CharacterTitle = "Mayor"
“It is not every day a corpse comes to speak to me. How may I be of assistance?” -> HubQuestions

=HubQuestions

+[<i>Show the Whispering Bell</i>]-> Artifact1
+{Artifact2Intro} [<i>Show the Apparition Gauntlet</i>]-> Artifact2
+{hasMayorNote1}[<i>Show note about friend</i>]-> note1
+{hasMayorNote2}[<i>Show note about enigmatic eye</i>]-> note2
+[<i>Enough for now</i>]-> goodbye 

=Artifact1
~ CharacterTitle = "Mayor"
“Now, this is most intriguing.”
~ CharacterTitle = ""
<i>A foreign air fills the Mayor at the sight of the Bell, his posture gaining some form at the sight of the artifact.</i>
~ CharacterTitle = "Mayor"
“That much explains the buzz I have seen wash over the townsfolk as of late.”

“You’ve earned my commendation, albeit I doubt it holds much value in these times.”

“No doubt, however, that such an achievement rightfully deserves recognition, even deathless as you are. "

"Spelunking into the ruins of Radefell with horrors stalking its corridors seems amongst the least of pleasant ventures.”

+["It could be better."]-> better 
+["It could be worse."]-> worse 
+["It is unpleasant, indeed."]-> unpleasent 

= better 
~ CharacterTitle = ""
<i>A blanched laugh breaches Mayor’s rosy cheeks, his eyes slitting at the long unfamiliar sensation.</i>
~ CharacterTitle = "Mayor"
“Haah, ah...it always could be. That, in a way, is why you seek these artifacts.”

“Each step you take is toward the untwisting of our reality. Each artifact you reclaim is another leap toward the ending of these senseless times.”

“Hold to that, and you will see our wrongs righted.”

-> Artifact2Intro 

=worse 
~ CharacterTitle = ""
<i>A breathless chuckle escapes from the Mayor’s chest, a genuine, slight opening in his smile showing teeth.</i>
~ CharacterTitle = "Mayor"
“Heh, heh, that is a decent disposition to possess. Perhaps it will lighten you enough to lighten your step.”

“Nevertheless, hold to that optimism, my friend. It will be as effective a tool as any artifact, if you allow it so to be.”

-> Artifact2Intro 

= unpleasent
~ CharacterTitle = ""
<i>The Mayor expels the humored thought through his nose, one half of his lip curling into a true smile.</i>
~ CharacterTitle = "Mayor"
“Then further respect for your efforts. I do not doubt that the work capable of redeeming ourselves or our world would ever be easy.”

“Yet...if enduring such an endeavor will cleanse the Malignance, then it will be well worth undertaking. You have already made great strides along this path, and such strides show that you are capable of much more.”

“Hold to the course, and I do not doubt that you will see it through.”

-> Artifact2Intro

=note1
~ CharacterTitle = "Mayor"
“...Let me read it.”
~ CharacterTitle = ""
<i>You hold the letter out to the Mayor, whose hands tremble as his eyes regard the letter.</i>
~ CharacterTitle = "Mayor"
“Where did you procure this? Within the dungeon?”

“That can only mean...”
~ CharacterTitle = ""
<i>The Mayor grits his savorless teeth, a dry frost settling his gaze low.</i>
~ CharacterTitle = "Mayor"
“...My thanks for bringing this to me, my friend.”
~ CharacterTitle = ""
<i>With a downward tilt of his head, the Mayor’s face is locked in shadow, a tremble still visible at the tip of his nose.</i>
~ CharacterTitle = "Mayor"
“Please, forgive my upset. It is simply just...”
~ CharacterTitle = ""
<i>The Mayor’s eyes seem to weaken as he holds the letter out. You take it from his hand, and his arm folds close to his chest.</i>
~ CharacterTitle = "Mayor"
“The implication of such a discovery is another pain I will bear.”

“I had hoped when I did not see him or his family here, that he was swifter than I.” -> letter1Questions 

=letter1Questions

+["Who was he?]" -> whoWasHe
+["Why did you warn him?"]-> WarnHim
+["What now?"]-> WhatNow

=whoWasHe 
~ CharacterTitle = ""
<i>A distant light twinkles in the Mayor’s eye, and youthful air fills his chest.</i>
~ CharacterTitle = "Mayor"
“A friend whom I once loved as a brother.”

“If I could turn back time,”

“I would have never let such kinship fade.”

-> letter1Questions

=WarnHim 
~ CharacterTitle = "Mayor"
“On that fateful day, I finally broke the Church’s leash. Station and position carries little value in the face of death.”

“It was too late, however, to speak out against the false truth the Church peddled. Radefell was too fervent in the belief that its salvation lay in the bringing of what we now know as the Malignance.”

“I only hoped that one would heed me, for respect of all those years past.”

-> letter1Questions

=WhatNow 
~ CharacterTitle = ""
<i>The Mayor inhales through his nose as he reforms his posture.</i>
~ CharacterTitle = "Mayor"
“There will forever be things we will wish to change from our pasts, yet the truth of the past can never be altered.”

“All that remains is to seize the present, and to strive toward a future we can be proud to call our past.”

“That is why I have chosen to remain. Since my history is marked with inaction, I will gift any mote of good I can to the future of Radefell.”

“Nevertheless, Is there anything you require?”

-> HubQuestions

=note2 
~ CharacterTitle = "Mayor"
“Allow me, please.”
~ CharacterTitle = ""
<i>You hand the Mayor the page. His eyes squint and his lips frown in recognition.</i>
~ CharacterTitle = "Mayor"
“Indeed, yes. This is my penmanship. I was witness to one of the artifacts, before the Convergence.”

“What do you wish to know?”

-> Letter2Questions 

=Letter2Questions 
+["What is this artifact?"]-> artifactQuestion
+["When did you see it?"]-> seeIt 
* -> HubQuestions

=artifactQuestion
~ CharacterTitle = "Mayor"
“The retainer who demonstrated the artifact called it the Eye of Genesis.”

“They claimed it would unite the powers of the artifacts, and bring form to the Malignance.”

“I cannot comprehend the extent of its power, only that it possesses it.”

“And its gaze...I hesitate to match its gaze again.”

-> Letter2Questions

=seeIt 
~ CharacterTitle = "Mayor"
“The morning of the Festival, hours before the Convergence. As the menace of the artifact washed over me, the revelation of horror that would befall us would come too late.”

“All of Radefell looked toward the Church as their saviors, and none heeded my warning as I raced through the streets.”

-> Letter2Questions

= Artifact2Intro
~ CharacterTitle = "Mayor"
“Nevertheless, there is still much work for myself to do. No doubt the same for you, with other artifacts to collect.”

-> questions

= Artifact2 
~ CharacterTitle = ""
<i>You see surprise tug the lids of the Mayor’s eyes open, and his stitched lips file into an impressed line.</i>
~ CharacterTitle = "Mayor"
“Now this is...most auspicious.”
~ CharacterTitle = ""
<i>The Mayor’s sight seems to settle on something that isn’t you for a moment, his next words half-spoken as his mind drifts.</i>
~ CharacterTitle = "Mayor"
“No doubt such diligence deserves recognition.”

+["Glad to be of service."]-> service 
+["I deserve no thanks."]-> thanks 
+["It is my duty."]-> duty
+["The work remains unfinished."]-> unfinished 

=service 
~ CharacterTitle = ""
<i>The Mayor’s gaze focuses on the wall behind you for a few seconds, until he looks back to you, coming to his senses.</i>
~ CharacterTitle = "Mayor"
“Service is a vast understatement.”

-> HubQuestions

=thanks
~ CharacterTitle = ""
<i>The Mayor’s gaze turns downward for a few seconds, until he looks back to you, coming to his senses.</i>
~ CharacterTitle = "Mayor"
“If you believe it to be so, then I will not impose my gratitude upon you. Just know that the course you took has brought bounty after the most brutal of famines.”

-> HubQuestions

=duty 
~ CharacterTitle = ""
<i>The Mayor’s gaze lies upon a way to his right for a few seconds, until he looks back to you, coming to his senses.</i>
~ CharacterTitle = "Mayor"
“That I cannot fault, even risen as you are. You have chosen it to be so nonetheless, and you have performed such a task with excellence.”

-> HubQuestions

=unfinished
~ CharacterTitle = ""
<i>The Mayor’s gaze seems unfocused for a few seconds, until he coughs and looks back to you, coming back to his senses.</i>
~ CharacterTitle = "Mayor"
“Quite, yes, yet there is still impact from what you have already done. Pride can be claimed in that.”

-> HubQuestions

= goodbye
~ CharacterTitle = "Mayor"
“I wish you well in your endeavors.”
-> DONE

=MayorBedrock
~ CharacterTitle = "Mayor"
<b>MAYOR</b>: "Greetings. How goes your endevors into the caverns of Radefell?”

+["It could be better."]-> betterBedrock
+["It could be worse."]-> worseBedrock
+["It is unpleasant, indeed."]-> unpleasentBedrock

= betterBedrock
~ CharacterTitle = ""
<i>A blanched laugh breaches Mayor’s rosy cheeks, his eyes slitting at the long unfamiliar sensation.</i>
~ CharacterTitle = "Mayor"
“Haah, ah… it always could be. That, in a way, is why you seek these artifacts.”
~ CharacterTitle = "Mayor"
“Each step you take is toward the untwisting of our reality. Each artifact you reclaim is another leap toward the ending of these senseless times.”

“Hold to that, and you will see our wrongs righted.”

-> END 

=worseBedrock
~ CharacterTitle = ""
<i>A breathless chuckle escapes from the Mayor’s chest, a genuine, slight opening in his smile showing teeth.</i>
~ CharacterTitle = "Mayor"
“Heh, heh, that is a decent disposition to possess. Perhaps it will lighten you enough to lighten your step.”

“Nevertheless, hold to that optimism, my friend. It will be as effective a tool as any artifact, if you allow it so to be.”

-> END 

= unpleasentBedrock
~ CharacterTitle = ""
<i>The Mayor expels the humored thought through his nose, one half of his lip curling into a true smile.</i>
~ CharacterTitle = "Mayor"
“Then further respect for your efforts. I do not doubt that the work capable of redeeming ourselves or our world would ever be easy.”

“Yet… if enduring such an endeavor will cleanse the Malignance, then it will be well worth undertaking. You have already made great strides along this path, and such strides show that you are capable of much more.”

“Hold to the course, and I do not doubt that you will see it through.”

-> END


==Clergy==
~ CharacterTitle = "High Priest"
{
 - isIntro: -> intro // Go to intro
 - isDeathF1: -> DeathF1 // Go to death 1
 - isHub: -> Hub// Go to hub
 - isDeathF2: -> DeathF2// Go to death 2 
 - else: Error -> END
 }
 
 /*
INTRO SECTION
*/
 
 = intro
VAR angryClerics = false
~ CharacterTitle = "Weeping Cleric"
"We face failure! How dare we yet breathe while the Perfect One suffers below..."
~ CharacterTitle = "Smiling Cleric"
"No, brother, we face success! The Perfect One has taken the city and dragged it beneath the lofty reaches of our sacred spire!"
~ CharacterTitle = "Thinking Cleric"
"We may still face either fate, my kin, but be aware, a fallen cleric listens."
~ CharacterTitle = "Weeping"
"Do you come to beg forgiveness?"
~ CharacterTitle = "Smiling"
"Do you come to share our mirth?"

~ CharacterTitle = ""
 <i>You look between each face, crossing your arms.
 *  ["I seek the artifact."]
    -> ADoomedQuest
 *  ["I seek a setting sun on your god."]
    -> TrulyDisgraced
 *  ["I seek nothing but safe passage below."]
    -> ADoomedQuest

= ADoomedQuest 
~ CharacterTitle = "Weeping"
"A doomed quest..."
~ CharacterTitle = "Thinking"
"...but a useful experiment."
~ CharacterTitle = "Smiling"
"Of course, disgraced one, you may pass into our lord's lair and meet his children. May it rekindle your faith."

-> GoForth

=TrulyDisgraced 
~ angryClerics = true
~ CharacterTitle = "Weeping"
"Disgraceful, as expected..."
~ CharacterTitle = "Smiling"
"And what a grand promise! There will be equally grand humor in your failure."
~ CharacterTitle = "Thinking"
"Poor heretic... we will speak to you no longer. Your empty words waste time which could have been spent in silence."

-> GoForth

= GoForth
~ CharacterTitle = "Thinking"
"<i>Asitotheh ko’ila pri’on anikoli</i>, may you fear that which possesses powerful senses, foolish child. Now, <i>fi</i>, {angryClerics:begone.|go forth.}"
    -> END

 /*
PLAYER DIES IN FLOOR 1 SECTION
*/

= DeathF1 
~ CharacterTitle = "Weeping Cleric"
"I hear shameful footsteps upon the sacred marble floors."
~ CharacterTitle = "Smiling Cleric"
"Of course! The wretched dreamer returns with broken spirits!"
~ CharacterTitle = "Thinking Cleric"
"I wonder, will it continue to flail in misguided misery... or has it come here to bathe in the holy light?"
~ CharacterTitle = ""
<i>The condescension drips sickly sour off of their lips as their eyes alight upon you. The grand cathedral around them seems to mock you, too, as their words reverberate off the walls. You can almost imagine the sound of a chuckle from the dark throne cloaked in shadow at the opposite end of the vaulted hall.</i>
 *  ["Let me pass, petulant ones. I care not for your distractions."]
    -> GoForth2
 *  ["I have no need for holy light, but I could use some answers."]
    <i>The thoughtful cleric spreads their arms wide.</i>
    ~ CharacterTitle = "Thinking"
    "We have little to hide. As long as you remain diplomatic, we will answer any questions you possess."
        -> AskQuestions

= AskQuestions 
~ CharacterTitle = ""
 *  ["Who are you?"]
    -> WhoAreYou
 *  ["Why are you still here?"]
    -> WhyStillHere
 *  ["Do you know of the other villagers?"]
    -> KnowVillagers
 *  ["The church was supposed to survive the cataclysm. What happened to everyone else? Where is the High Priest?"]
    -> WhatHappenedToChurch
 *{CHOICE_COUNT() < 4}->  
 ["Enough. I wish to pass on into the dungeon."]
    -> GoForth2

= WhoAreYou 
~ CharacterTitle = "Weeping"
"I am Ila, acolyte of repose."
~ CharacterTitle = "Smiling"
"I am Ina, acolyte of beast."
~ CharacterTitle = "Thinking"
"I am Ana, acolyte of word. Who are you?"
 *  [I am the endless repose.]
 ~ CharacterTitle = "Weeping"
    "Doubtful..."
 *  [I am the lonely beast.]
 ~ CharacterTitle = "Smiling"
    "Laughable!"
 *  [I am the final word.]
 ~ CharacterTitle = "Thinking"
    "Intriguing."
 *  [I am the Disgraced.]
 ~ CharacterTitle = "Thinking"
    "Without a doubt."
- -> AskQuestions

= WhyStillHere 
~ CharacterTitle = "Weeping"
"How could we dare to abandon our post?"
~ CharacterTitle = "Smiling"
"Why would ever want to leave this perfect place?"
<i>The thoughtful cleric considers you for a moment...</i>
~ CharacterTitle = "Thinking"
"You forget the predicating question, young one. Where would we go?"
-> AskQuestions

= KnowVillagers 
~ CharacterTitle = "Thinking"
"Of course, our spire watches over each of them."
 *  ["What are your thoughts on the Crypt Keeper?"]
 ~ CharacterTitle = "Weeping"
    "I know her...Heathen. Traitor."
    ~ CharacterTitle = "Smiling"
    "Laughable! A true failure!"
    ~ CharacterTitle = "Thinking"
    "...and a troublingly talented woman."

 *  ["What are your thoughts on the Mayor?"]
 ~ CharacterTitle = "Weeping"
    "Of course... that-"
    "Wait, who?"
    ~ CharacterTitle = "Thinking"
    "The weak-willed one, didn’t he flee?"
    ~ CharacterTitle = "Smiling"
    "Oh, that fun little puppet! He ran from the city at the first sign of our lord's rise!"

 *  ["What are your thoughts on the Scholar?"]
 ~ CharacterTitle = "Thinking"
    "They were an adept keeper of the church's secrets, even the High Priest adored their fervor."
    ~ CharacterTitle = "Smiling"
    "What a monster for knowledge!"
    ~ CharacterTitle = "Weeping"
    "...and what a tortured soul."
    
- -> AskQuestions


= WhatHappenedToChurch 
~ CharacterTitle = ""
<i>The clerics visibly flinch at your question.</i>
~ CharacterTitle = "Weeping"
"The church lives, so magnificent, though tears flow from her spires..."
~ CharacterTitle = "Smiling"
"...and we live, too, blessed to be the final children of the Malignance..."
~ CharacterTitle = "Thinking"
"...and the High Priest lives, most holy, slumbering in the great throne, awaiting the proper calalyst to their return."
"Your inquiry cuts blunt and foolish, question us no more."

-> GoForth2

= GoForth2
~ CharacterTitle = "Smiling"
"We will see you again soon, regardless of whether success or failure sinks its fangs into you first."
~ CharacterTitle = "Weeping"
"Though I do not have hope in your ability to escape the cold embrace of death any time soon. Mortality cannot be cheated forever..."
    -> END


 /*
PLAYER SUCCEEDS IN FLOOR 1 SECTION (?? will the other things take precedence over hub? hopefully hub will only ever be triggered once (LOL IVY -ivy))
*/

= Hub 
~ CharacterTitle = ""
<i>The great church has become consumed in a whirl of dark whispers. The chanting voices of the three clerics fill the air.</i>
~ CharacterTitle = "Clerics"
"<i>Asi'ona! Asi'ona! Asi'ona! Fi'a!</i>"
~ CharacterTitle = "Thinking Cleric"
"Perfect One, our great lord, god of gods, creator of ruins, destroyer of all things not yet so. Fi'a! The heretic has stolen what belongs to you, come forth!'
~ CharacterTitle = ""
<i>The earth shakes and the air tastes of metal.</i>

<i>A dark, humanoid form rises from the throne behind the clerics, booming laughter filling the air. The figure approaches.</i>
~ CharacterTitle = "High Priest"
"Your transgressions mark a black stain upon the lightless void. Identify yourself, heretic, among the three: are you traitor, puppet, or monster?"
 *  ["I am no traitor."]
 *  ["I am no one's puppet."]
 *  ["I am not a monster."]
 ~ CharacterTitle = "High Priest"
-"Maybe not, but you are weak. Though you have stolen from my dungeon, you have only shown the futility of your quest."
"My children grow hungrier. Your final death is imminent."
"Now, get out of my sight."
    -> END


 /*
PLAYER DIES IN FLOOR 2 SECTION
*/

= DeathF2 
~ CharacterTitle = ""
<i>The menacing figure of the High Priest turns to you, the other clerics cowering behind.</i>
 ~ CharacterTitle = "High Priest"
"My clerics forewarned me of the great humor of your repeated failings, but nothing could have prepared me for the reality of your humiliation."
  *  ["Enough mocking, I have questions."]
  ~ CharacterTitle = ""
    <i>The High Priest's black eyes burn twin holes in your head, but they do not decline your request.</i>
        -> AskQuestionsOfHP
 *  ["I will not speak to you any more than I must, fear-father. Let me pass."]
    -> GoForth3

= AskQuestionsOfHP 
 *  ["What are you?"]
    -> WhatAreYou
 *  ["What do you think of the villagers?"]
    -> WhatOfVillagers
 *  ["You returned when I discovered the Magic Hand, why? What do you want?"]
    -> WhatYouWant
 *  ["Enough. I wish to pass on into the dungeon."]
    -> GoForth3

= WhatAreYou 
 ~ CharacterTitle = "High Priest"
"I am a vision of myself, wrenched from my infinite form into this weak body."
~ CharacterTitle = ""
<i>Chills creep up the back of your neck. The High Priest's cold stare holds you, immobilized.</i>
 ~ CharacterTitle = "High Priest"
"A better question, I ask myself: what are you?"
"Death-cheater, oath-breaker, disgraced. Despite your resistance, you are destined to be nothing, just as I am destined to be everything."
"Fate will not relent for you, and time will reveal my true nature."
- -> AskQuestionsOfHP

= WhatOfVillagers 
 ~ CharacterTitle = "High Priest"
"They are, all but one, wretched invertibrates. Which of them interests you?"
 *  ["The Crypt Keeper."]
  ~ CharacterTitle = "High Priest"
    "She is a betrayer, heathen, and the most egregious of failures, despite her talents."
 *  ["The Mayor."]
  ~ CharacterTitle = "High Priest"
    "He is a weak-willed puppet, and it was delicious fun to toy with him before he fled during my rise."
 *  ["The Scholar."]
  ~ CharacterTitle = "High Priest"
    "They are a poor, tortured monster, and I cannot help but adore their fervor. They are also quite a talented keeper of secrets, to be sure."
- -> AskQuestionsOfHP

= WhatYouWant 
 ~ CharacterTitle = "High Priest"
"You've brushed so close against success..."
"I want to bear witness to when you inevitably fail."
-> AskQuestionsOfHP

= GoForth3 
 ~ CharacterTitle = "High Priest"
"Of course... I will see you again shortly, doomed heretic."
    -> END
==Scholar==
 
 =Hub
 ~ CharacterTitle = "Scholar"
 {
 - isScholarIntro: -> ScholarhubIntro// Go to intro
 - isHub or isDeathF2: -> regularHub// Go to hub
 - else: Error
 }

=ScholarhubIntro
{ScholarhubIntro > 1: -> ScholarBedrock}
 ~ CharacterTitle = ""
<i>Despite your misgivings, you walk towards the uninviting building. </i>

<i>As you approach, the door creaks open, and a robed figure appears from the deep shadows of the doorway. </i>

<i>The little of their face left uncovered by bandages sends a shiver down your spine. </i>

<i>And yet...a wave of familiarity crashes upon you all the same. </i>

<i>The gravelly voice from the person before you echoes louder in your head than in your ears.</i>
 ~ CharacterTitle = "Scholar"
Hmm, so it appears one has returned...
 ~ CharacterTitle = ""
<i>It has been a long while since I’ve conversed with someone... of a similar origin. </i>
 ~ CharacterTitle = "Scholar"
It’s good to know we few are not the last.

*["What do you mean?"]-> WhatDoYouMean 
*["It is good to see you too."]-> GoodToSee

=WhatDoYouMean 
 ~ CharacterTitle = "Scholar"
Hmm? Has the wanderer lost themself in their years away? Surely, they must recognize one of their Church’s great knowledge keepers.

Not much for talk, wanderer? I can’t say I’m surprised. The eternal enigma they are, even to one as learned as I. 

-> introQuestions 

=GoodToSee 
 ~ CharacterTitle = ""
<i>You give a deep bow. </i>
 ~ CharacterTitle = "Scholar"
Ahh, such respect from the wanderer.

Nothing less than I’d expect from Malisense’s most faithful.

It seems, even after all this time, they remember how to respect their superiors.

-> introQuestions

=introQuestions
 ~ CharacterTitle = "Scholar"
But, niceties and remembrances aside, I doubt idle prattle is what the wanderer seeks. What is it they wish of me?

*["What happened to you?"]-> toYou 
*["What happened here?"]-> happenedHere 
*-> introGoodbye 

=toYou 
 ~ CharacterTitle = "Scholar"
Isn't that an interesting quandary. 
And the answer is even more intriguing.
But, forgotten wanderer, I hardly wish to reveal such knowledge to one who has become practically a stranger. 
Perhaps some other time I shall tell, but for now the doors to my secrets are locked.

-> introQuestions

=happenedHere 
 ~ CharacterTitle = "Scholar"
I take it the wanderer fled before the damned city fell? 

Well, in essence, after the Ritual that day, the great evil that once laid beyond our limited mortal grasp finally gained physical form, and with that form decided to drag humanity down to the damnation it felt we deserved. 

In an instant, the city disappeared beneath the Earth, and those who remained either lost their minds or their freedom to the Malignance. 

Evidently, some people were able to escape the destruction–though none unscathed–and decided to go about the process of rebuilding...or whatever the closest thing might be. 

I doubt the Radefell we knew shall ever stand as it did before...

-> introGoodbye 

=introGoodbye
 ~ CharacterTitle = ""
<i>They let out a heavy sigh.</i>
 ~ CharacterTitle = "Scholar"
It has been so long since I have spoken so much to another. 

But, I assume the wanderer has something somewhere to attend to. So, leave, then. 

Come again with any items of interest, and I shall tell of their nature. 

And...give my regards to the Crypt Keeper. If she's still around.
-> END

=regularHub
{regularHub > 1: -> ScholarBedrock}
 ~ CharacterTitle = ""
    <i>As you approach the stout building once more, the dimly-lit windows signal to you that the Scholar is at home.</i>
    *[Knock on the door]
    -> ArtifactTibits
    
= ArtifactTibits 
{ArtifactTibits < 1 : -> ArtifactTibits1}
{ArtifactTibits < 2: -> ArtifactTibits2}
{ArtifactTibits > 2: -> ArtifactTibits3}
 ~ CharacterTitle = "Scholar"
{hasDied: Ah, what joy! The wanderer returns once again! I had worried our last meeting would be our final one. What a tragedy that would have been... -> ArtifactQuestions}

= ArtifactTibits1
 ~ CharacterTitle = "Scholar"
The wanderer is eager to know more? Please, enter my domicile and pilfer away at my wealth of knowledge. 
Let us fill your mind with forgeries.
I know the wanderer seeks to know more of the artifacts...I shall share my burden with the wanderer so that it no longer crushes solely my shoulders.
There are three. The Whispering Bell, the Apparition Gauntlet, and the Eye of Genesis. -> ArtifactQuestions

= ArtifactTibits2
 ~ CharacterTitle = "Scholar"
Back already, hm? I am pleased the wanderer's steps have not ceased along their journey, and have instead brought them back to me.-> ArtifactQuestions

= ArtifactTibits3
 ~ CharacterTitle = ""
<i>A faint memory emerges from the deep crevaces of your mind. You're sitting alone in an office.</i>

<i>You must've been seven or eight years old. The Scholar helping you with your studies. </i>

<i>They've made little finger puppets, which they're now using to try and explain the Old Wars to you. Where did that cheery person go? Maybe they never left.</i> -> ArtifactQuestions

 

=ArtifactQuestions 
 ~ CharacterTitle = "Scholar"
What do you wish of me?

 *  ["What is the Whispering Bell?"]
    -> WhisperingBell
* {WhisperingBell} ["May I show you something?"]
    -> AfterA1

 *  ["What is the Apparition Gauntlet?"]
    -> ApparitionGauntlet
* {ApparitionGauntlet} ["May I show you something?"]
    -> AfterA2

 *  ["What is the Eye of Genesis?"]
    -> EyeofGenesis
    
 *  ["I'm done here."]
    -> END

*-> CharacterQs


= WhisperingBell 
 ~ CharacterTitle = "Scholar"
According to legend, the Whispering Bell was formed deep underground–much like a diamond. 

Over time, the bell was pushed closer and closer to the surface, until it was eventually found by the great General Dymilos. 

Many historians attribute Dymilos' strategic prowess to the bell, which he claimed allowed him to see enemy soldiers through castle walls and country hillsides. 

The battles he won were the crux of Radefell's glory. Our city would not have held as long as it did without his strength. 

When he died, the bell was taken by the Church, as revered as a sacred item. 

Upon the death of a devotee, the bell would be rung once in honor of their life lived in sacrifice to the Church. 

The wanderer must remember its tolls, even if they knew not from where. And it seems the bell has yet to toll for them. 

Keep it that way, dear wanderer. 

-> ArtifactQuestions

= ApparitionGauntlet 
 ~ CharacterTitle = "Scholar"
Indeed, the Apparition Gauntlet! An item of great power, and greater mystery. I admit, despite my position as Radefell's last great Scholar, even I have very little knowledge of its true origins. 

However, many legends agree that this artifact sprung from the earth, as if grasping for the light of the surface it knew only whispers of. 

Supposedly, it's outstretched grasp is fueled by the anger of the souls sacrificed to the Malignance, so much so that they can still influence the mortal world. 
-> ArtifactQuestions

= EyeofGenesis 
 ~ CharacterTitle = "Scholar"
The Eye of Genesis...once a seemingly mundane pendant, it was adorned with a curious eyeball. 

This was originally utilized by the Church as a way to oversee everyone within it. Some say its ever watchful gaze is constantly predicting the next movements of all within its line of sight. 

As for how it tells the clergy about misbehavior or imminent issues, that is a closely guarded secret of the Church that only the High Priest and few others knew. 

-> ArtifactQuestions

= AfterA1 
 ~ CharacterTitle = "Scholar"
Ahh, it seems our intrepid wanderer has brought back something interesting. Brilliant, brilliant. 

I hope the wanderer does not mind my writing as we talk. My mind has been clouded since Radefell perished, and nearly I along with it. 

Writing helps me remember...remember what I am, what it was, and who we are. 

Now, what is it that the wanderer wishes to show me?
 ~ CharacterTitle = ""
<i>You show them the bell. </i>

<i>Instantly, the Scholar's remaining eye lights up. </i>

<i>You've never seen them in such a state. </i>

<i>Their excitement makes them seem almost monstrous, their hand quickly scribbling unrecognized symbols on their parchment. </i>
 ~ CharacterTitle = "Scholar"
My, my, my, what have we here? Such an incredible item they seem to have brought to show me! 

To the wanderer's layman eyes, I imagine it appears to simply be a bell. 

What was their clerical title, again? Well, no matter, they have earned my knowledge of this artifact. 

* ["Please, tell me more."]
 -> tellMore
 
* ["Just get on with it, already."]
 -> getOn

= tellMore  
 ~ CharacterTitle = "Scholar"
It would be my greatest pleasure.
 ~ CharacterTitle = ""
<i>You hold the mysterious bell up to the Scholar. </i>

<i>Though their expression remains concealed, you can feel the giddiness rising within them as they hungrily take in every inch of the artifact. </i>
 ~ CharacterTitle = "Scholar"
Ah, yes. Yes! What serendipity! The wanderer has brought to me the Whispering Bell!
-> WhisperingBellExplination

= getOn 
 ~ CharacterTitle = "Scholar"
Such impatience. The wanderer must learn to wait... 
 ~ CharacterTitle = ""
<i>They begin eyeing the bell thoroughly. Though their expression remains concealed, you can feel the giddiness rising within them as they hungrily take in every inch of the artifact... </i>
 ~ CharacterTitle = "Scholar"
Certainly this cannot be...Yet, it is...wanderer. Oh, gracious wanderer! You have brought to me the Whispering Bell! 

-> WhisperingBellExplination

= WhisperingBellExplination
 ~ CharacterTitle = "Scholar"
Mortal minds cannot hear the sound of the bell. If I could, I shudder to think of how terrible it might sound. 

Seeing it here, in the flesh, I can confirm a few suspicions that it would have been unbecoming of me to voice before. 

There were rumors it was used to track signs of the Malignance when rung, and it thusly must allow you to track the souls corrupted by it. As for a reward for showing me this, I shall tell you of my folly, and of the knowledge I have burdened myself with.

*["Your folly?"]-> folly 

=folly 
 ~ CharacterTitle = "Scholar"
I seek memories of the Convergence, to preserve my own humanity, as I now have the unfortunate position of feeling empathy for the Malignance, yet at least I hold my own memories. 

I lost my peers, and my own control over my body, and all for what? 

To have no mouth with which to speak my knowledge, to bear the burden of these secrets alone...at least I may tell you without any fear. That is all I will say for now. 

Now, go and wander once more, down in the dark and despair of the Malignance's tomb. 

The ringing echoes of the dead shall clear the way of the corrupted.

-> END

= AfterA2 
 ~ CharacterTitle = "Scholar"
Brilliant, simply brilliant. The wanderer never fails to impress, as I should have expected. It seems even I have forgotten why they were so close to the heart of our Church... 
 ~ CharacterTitle = ""
<i>You hold up the Apparition Gauntlet.</i>

<i>As the gilded hand shimmers in the faint light before the doorway, you see its gleam reflected in the Scholar's eyes. Their excitement is apparent. </i>
 ~ CharacterTitle = "Scholar"
It...This is...wanderer. My dear wanderer! What blessings they bring to my doorstep, for they have found me the Apparition Gauntlet!

To think that such a legendary artifact is here before me now. 

 * ["Please, tell me more."]
 -> tellMoreGautlet 
 
* ["Just get on with it, already."]
 -> getOnGautlet
 
 =getOnGautlet 
  ~ CharacterTitle = ""
 <i>The Scholar's brow furrows slightly.</i>
  ~ CharacterTitle = "Scholar"
 It seems I have also forgotten their lack of patience. But, no matter. Bring it here.
 
 -> tellMoreGautlet
 
 = tellMoreGautlet 
{-getOnGautlet: 
 ~ CharacterTitle = "Scholar"
But, no matter. Bring it here.
-else:
 ~ CharacterTitle = "Scholar"
If you'll allow me...
}
 ~ CharacterTitle = ""
<i>The Scholar reaches over to touch the Gauntlet, then shudders. </i>
 ~ CharacterTitle = "Scholar"
It is as I thought, my intuitions were correct...

-> handQuestions

=handQuestions

*["How should I use it?"]-> howUseHand 
*["What was its use?"]-> useHand
*-> handGoodbye

=howUseHand 
 ~ CharacterTitle = "Scholar"
It seems the souls consumed by the Malignance still claw for the surface of a tempestuous sea of their own multitudes. 

Their outstretched grasp and its lack of a grip on reality ends up overcompensating, so that its infrequent touch far outstretches the physical bounds of the Gauntlet's fingers. 

For those with a vocabulary lesser than mine, it may touch others from very far away in its attempts to cling to the mortal realm.

-> handQuestions



=useHand
 ~ CharacterTitle = "Scholar"
This forces me into a position of conjecture given how much of the information about it is knowledge or rumor, but considering the truth of it being the consumed souls of the Malignance vying for revenge...

It must have been used by the Church for the controlling of beasts...still, as thanks for sharing such a wonderous creation with me, I shall speak more of my senselessness...

If I had merely left the clergy like the Crypt Keeper, my body would not be in such a state as this...oh, what a fool I've been. 

*["Your body?"]
 ~ CharacterTitle = "Scholar"
I...do not know if it is safe to share this secret but...my body is not my own. 

I only make use of the eye and my mind. I am left as a puppet, much like the Mayor. 

N-Not in the political sense or whatever is going on with that coward. 

It was a great mistake of mine, to give up my body to a beast that controls it...I am grateful you are the only visitor I get. 

I shall speak no more of it to you....
-> handQuestions

=handGoodbye 
 ~ CharacterTitle = "Scholar"
Now, go and wander once more, down in the dark and despair of the Malignance's tomb. 
-> END 

= CharacterQs 
* ["What do you know about the Crypt Keeper?"]
-> ckAnswers

* ["What can you tell me about the Mayor?"]
-> mayorAnswers

* ["Anything to say about the clergy?"]
-> clergyAnswers

* ["Enough questions."]
-> END

= ckAnswers 
 ~ CharacterTitle = "Scholar"
So you wish to know more about the Crypt Keeper...I must say, there is some knowledge I am not burdened by. 

I know not if it is my place to say this, but rightfully she left the clergy long ago while I remained, blinded by my search for knowledge. 

 * ["Why did she leave?"]
 ~ CharacterTitle = "Scholar"
There were times where the good she could do was limited by being in the Church. And so she left, for a better life. And look where it got her in comparison. 
 ~ CharacterTitle = ""
 <i>They reach to touch the bandages on their face, then shudder.  </i>
 ~ CharacterTitle = "Scholar"
T-That is enough. Get on with any other questions you have. But please, send the Crypt Keeper my regards. She did the right thing.

-> CharacterQs

= mayorAnswers 
 ~ CharacterTitle = "Scholar"
The world of politics was rather out of my purview of knowledge, I know not the intricacies of negotiation and I most certainly never have known the intricacies of fleeing like a coward. 

I have done many things that were senseless, but never as senseless as that fool of a mayor. 

 *["Senseless?"]

The wanderer seeks to know of my many mistakes, but I shall only speak of it if you show me any and all artifacts you collect. 

-> CharacterQs
= clergyAnswers 
 ~ CharacterTitle = "Scholar"
The clergy...all I know is that they are members of the Church, with the exception of the High Priest...they kept me on a short leash, treating me like a curious infant, even when I was one of the foremost keepers of Church secrets. 

Perhaps they were right that I would never learn some sort of higher truth, and I never left the clergy like the Crypt Keeper did...if I had, maybe I'd have learned some greater truth.

-> CharacterQs

=ScholarBedrock
~ CharacterTitle = "Scholar"
Ah, you have returned. Tell me, what has given you trouble?

+["The monsters are difficult."]
Do not forget of the items at hand. These wonderful novelties may be the difference between having an artifact in hand, or being awokened in the arms of the Crypt Keeper.
-> ScholarBedrock

+["Gems are hard to find."]
Hmmm...
By my research, the gems are known to be at the outskirts of the city.
Do what you will with that information.
-> ScholarBedrock

+["I am not having difficulty."]
Interesting response.
I did not know you were a liar, wanderer.
~ CharacterTitle = ""
<i>They quickly scribble something on a nearby paper.</i>
~ CharacterTitle = "Scholar"
"Noted."
-> ScholarBedrock

+["Goodbye."]
Till next time, wanderer.
    -> END


==End== 
~background = "next"
~ CharacterTitle = ""
<i>You burst into the Mayor’s house, gripping the note in a fierce clutch. You betray your lightfooted step and close the distance across the room in an instant, locking eyes with the Mayor. </i>

<i>While he does not flinch from your gaze, the Mayor’s lips are pressed together, but not smiling this time.	</i>
~ CharacterTitle = "Mayor"
“I knew this time would come...”

+["You have the final artifact!"] 
~ CharacterTitle = "Mayor"
“Indeed. It is within my possession, and I am willing to tell you where it is.”  

++["Tell me."]
~ CharacterTitle = "Mayor"
“I hid it within the fountain, at the center of town. You can find it there.”

+++["Tell me what happened."]-> Explination 
+++["I am done with you."]-> CryptKeeperFinal

= Explination
~ CharacterTitle = "Mayor"
{Mayor.note1: “You know the story, you found that scrap of my journal. Allow me to illuminate the full context.”}

“Even as I acted toward the supposed good of Radefell, I was but an unknowing puppet for the Church, unaware of what would come until it was too late.”

“The evening before the Convergence, the Church channeled the gall to demonstrate the Eye of Genesis to me. They must have known that not a soul would heed me, that I would tarnish my image in my doomsaying and they would rise to establish their theocracy over Radefell.”

“We know now that it would have never been that simple.”

+["What happened before the Convergence?"]-> PreConvergence
+["I am done with you."]-> CryptKeeperFinal

=PreConvergence
~ CharacterTitle = "Mayor"
“I toiled, unable to come to a course of action that could stop the Church. I am no warrior nor sneak, so I had no chance of infiltrating where they kept the Eye of Genesis.”

“So...I simply waited. Listening to the final celebrations of many lives. Even though I foresaw the apocalypse that would unfold, I could not predict the specificity of its horror.”

“As the Malignance began to break through and across our reality, I ran into the Church’s doors, wrote the note, and ran like fire with the Eye, hoping that I or another would discover a method for aiding against the Malignance in the future.”

“I passed those screaming to hear their own voices, so many toppled over, locked in a hell within their body. Unable to move, unable to see, losing the ability to know what horror had befallen us.”

“In that flight from Radefell, the chance to aid those who I warned laid before me, and I left them to their fates.”

“I damn myself for each and every soul I abandoned that night.”

+["There was nothing you could do."]-> NothingToDo
+["I am done with you."]-> CryptKeeperFinal

=NothingToDo
~ CharacterTitle = ""
<i>The Mayor’s gaze flits low, his dry teeth clenching nearly hard enough to crack.</i>
~ CharacterTitle = "Mayor"
“I cannot accept any excuse for my cowardice, my friend. It is my failure, and mine alone. I will give myself to whatever good I can accomplish, but I know it will never pay the full tithe of my sin.”

“Especially after this final negligence...”

+["Deservedly so."]-> Deserved
+["Leave the past behind."]-> behind
+["I am done with you."]-> CryptKeeperFinal

=Deserved
~ CharacterTitle = ""
<i>The Mayor vents a tasteless breath, a pained smile plastered upon his face.</i>
~ CharacterTitle = "Mayor"
“Indeed, my friend.”

+["Why didn’t you tell me where the Eye is?"]-> WhyDidntYouTellMe
+["I am done with you."]-> CryptKeeperFinal

=behind 
~ CharacterTitle = ""
<i>The Mayor smiles, the thin stitch of his lip trembles as his eyes seem to vibrate in his skull.</i>
~ CharacterTitle = "Mayor"
“I...I do not know if I can, my friend. The faces, lost and twisted with the horror of their very existence deprived from them, they haunt me every time I close my eyes.”

“Even further, there is one final negligence of mine...”

+["Why didn’t you tell me where the Eye is?"]-> WhyDidntYouTellMe
+["I am done with you."]-> CryptKeeperFinal

=WhyDidntYouTellMe
~ CharacterTitle = "Mayor"
“Knowing your task of collecting the artifacts, I realized that my displacement of the Eye of Genesis would need to be revealed.”

“I did not yet know your capability when you arose, and there was the potential outcome of you losing it in the depths of Radefell where it would be out of reach. I kept it secret then, and I would reveal it to you once you had returned with the second artifact.”

“But then...hope came to the village. I enjoyed working with our fellows, and a distant contentment finally came to me.”

“I thought that with the unveiling of the Eye’s location and the cleansing of the Malignance, it would come time to bring those who allowed the Malignance’s proliferation to trial.”

“So, instead, I opted not to inform you. I could enjoy the little, happy life here until it would be time to come clean.”

“And that time has come. I won’t pretend that such actions were not selfish.”

-> Selfish

=Selfish
*["Why do it then?"]-> WhyDoIt
*{WhyDoIt}["You deserve to be punished."]-> punish
*{WhyDoIt}["I would have protected you."]-> protected
*{punish} or {protected} ["There is one thing I wish you to do."]-> Wish
+["I am done with you."]-> CryptKeeperFinal

=WhyDoIt
~ CharacterTitle = ""
<i>The Mayor’s rosy cheeks reach a high blush, blended deep with shame.</i>
~ CharacterTitle = "Mayor"
“Perhaps, we can never escape our true nature, no matter how much we wish to change.” 

-> Selfish

= punish
~ CharacterTitle = "Mayor"
“I don’t disagree, I’m afraid.”

“I will accept whatever judgment comes to me. I will give whatever remains of my word to that.” 

-> Selfish

= protected
~ CharacterTitle = "Mayor"
“That is a boundless kindness. One that I could never have predicted.”

“One I would not deserve even had I revealed the Eye sooner.”

-> Selfish

=Wish
~ CharacterTitle = ""
<i>The Mayor nods, awaiting your word.</i>

+["Await your judgment here."]-> Judgement
+["Life free of the past."]-> Past
+["Submit to condemnation."]-> Condemnation
+["Give yourself to the future of Radefell."]-> GiveYourself

= Judgement 
~ CharacterTitle = "Mayor"
“Very well. I will do as you ask. Farewell, Disgraced.”

“End this curse, once and for all.”

-> CryptKeeperFinal

= Past
~ CharacterTitle = "Mayor"
“I...I will do my best to, my friend.”

“Go. End this curse, once and for all.”

-> CryptKeeperFinal

= Condemnation
~ CharacterTitle = ""
<i>The Mayor simply nods, then bows his head, closing his eyes as he whispers his last words.</i>
~ CharacterTitle = "Mayor"
“Go. End this curse, once and for all.”

-> CryptKeeperFinal

=GiveYourself
~ CharacterTitle = ""
<i>The Mayor’s stitched lip curls inward, but he nods.</i>
~ CharacterTitle = "Mayor"
“Very well, I can hold true to that. Farewell, Disgraced.”

“Go. End this curse, once and for all.”

-> CryptKeeperFinal


=CryptKeeperFinal
~ CharacterTitle = "Mayor"
~character = "Disgraced"
“Farewell then, Disgraced. End this curse, once and for all.”
~ CharacterTitle = ""
<i>You approach the fountain, discerning it closely for the artifact. </i>

<i>It isn't until you look closely into its waters that you spy a metal box, barely off-color with the stone, at the bottom of the fountain. </i>

<i>You bend and reach in, heaving the box out of the water, set it down, and open its lid.</i>

<i>Within the box lies The Eye of Genesis. </i>

~character = "Crypt_Keeper"
<i>Its gaze almost seems to apprise you as you take it into your hands. </i>

~ CharacterTitle = ""
<i>You hurry back to meet with the Crypt Keeper, the Eye of Genesis, the final artifact, in hand. </i>

<i>As you approach her, she remains silent, unmoving. Instead, she focuses on the barren land surrounding the town.</i>

*[<i>Get closer</i>]
    ~ CharacterTitle = ""
    <i>Her acknowledgement of your presence is betrayed only by tinge of sorrow that has appeared in her face.</i>
    **[<i>Sit down next to her</i>]
        ~ CharacterTitle = "Crypt Keeper"
        "You know what this means, right?"
        ***["Of course."]
            ~ CharacterTitle = "Crypt Keeper"
             "Yes, yes...You know what this means for the Malignance, you know what this means for Radafell..."
             ~ CharacterTitle = ""
                <i>Her tone of voice raising with every passing word.</i>
                -> WhatItMeans
        ***["I do not."]
            -> WhatItMeans
        ***["..."]
            -> WhatItMeans

==WhatItMeans
~ CharacterTitle = ""
<i>She takes up your hands, and finally makes eye contact with you. Her eyes water.</i>
~ CharacterTitle = "Crypt Keeper"
"Do you know what this means for you?"
*["For me?"]
    ~ CharacterTitle = "Crypt Keeper"
    "Yes! For you! You haven't sat down and thought about what all this means for you?"
    ~ CharacterTitle = ""
    <i>Her grip on your hands strengthen, averting her eyes once again.</i>
    **["I have not."]
        ~ CharacterTitle = "Crypt Keeper"
        "Of course you haven't. Of course you haven't...and that's why you were the only person who could have succeeded in this quest."
        ***["What does this mean?"]
            ~ CharacterTitle = ""
            <i>Any words that she tries to get out are choked up by her tears as she embraces you, burying herself into your chest. </i>
            
            ****[<i>Comfort her</i>]
                ~ CharacterTitle = ""
                <i>For a minute, she remains like this. You feel her shoulders heave up and down as she continues to silently weep with you in embrace.</i>
               
                <i>She looks back at you. </i>
                ~ CharacterTitle = "Crypt Keeper"
                "It...it's for the best though. Please, my peony, give me the artifact."
                 
                ******[<i>Give her the artifact</i>]
                    ~ CharacterTitle = ""
                    ~character = "Darkness"
                    <i>After taking the artifact, she embraces you one final time. She pulls away before she begins the cleansing ritual of the Eye of Genesis.</i>
                        
                    *******[<i>Breathe</i>]
                        ~ CharacterTitle = ""
                        <i>As the process goes on you start to feel tired, weak, like you can't...</i>
                            
                        ********[<i>Breathe</i>]
                            ~ CharacterTitle = ""
                            <i>The light fades. The only thing that you can envision are your memories.</i>
                                
                            -> SensesReturn

==SensesReturn==
*[<i>Look</i>]
    ~ CharacterTitle = ""
    <i>Your adventures, the monsters you've encountered, the people you've met, the city you've saved all flash before your eyes.</i>
    -> SensesReturn

*[<i>Smell</i>]
    ~ CharacterTitle = ""
    <i>You inhale sharply, smelling the flora of the city. The sharp fragrance reminds you of your past, and a newer, brighter future.</i>
    -> SensesReturn

*[<i>Taste</i>]
    ~ CharacterTitle = ""
    <i>Your mouth starts to water as you taste once again. Breads, meats, fruits, familiar flavors from unfamiliar places.</i>
    -> SensesReturn

*[<i>Hear</i>]
    ~ CharacterTitle = ""
    <i>You once again hear churchbells ring, birds chirping, a bustling city once again.</i>
    -> SensesReturn
*{CHOICE_COUNT() ==0} [<i>Feel</i>]
    ~ CharacterTitle = ""
    <i>You feel air escape you, the light from the cleansing grows brighter and you feel it's heat on you.</i>
    **[<i>Feel</i>]
        ~ CharacterTitle = ""
        <i>You feel a burning sensation, little pins and needles assaulting into your skin.</i>
        ***[<i>Feel</i>]
            ~ CharacterTitle = ""
            <i>You feel comfort, taking shelter in the warmth of the light, a familiar feeling. An embrace, her embrace.</i>
            ****[<i>Breathe</i>]
                ~ CharacterTitle = ""
                ~character = "Dark"
                <i>Then it goes silent.</i>
                ~ CharacterTitle = "Crypt Keeper"
                "The journey was long and treacherous."

                "The burden had been laid on your shoulders, and you succeeded."

                "At some point I thought I could bear loss, I could withstand saying goodbye."

                "But no pain was greater than letting you go."

                "The more life returns to Radefell, the more indebted we are to you."

                "The more your sacrifice means."

                "The more I miss you..."

                "My peony." -> END


-> END

