VAR isIntro = true
VAR isDeathF1 = false
VAR isHub = false
VAR isDeathF2 = false 
VAR isIntroductionCutscene = false
VAR isEnd = false
VAR hasDied = false

VAR isMayorIntro = true
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

== START ==
{
 - isIntroductionCutscene: -> Introduction // Go to Introduction
 - character == "Crypt_Keeper": ->Crypt_Keeper // Go to CK 
 - character == "Stick": -> Stick // Go to Stick
 - character == "Mayor": -> Mayor // Go to Mayor
 - character == "Clergy": -> Clergy // Go to Clergy
 - character == "Scholar": -> Scholar // Go to Scholar 
 - isEnd: // Go to end
 - else: Error
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
 - isIntro: ->intro // Go to intro
 - isDeathF1: -> DeathF1 // Go to death 1
 - isHub: -> hub// Go to hub
 - isDeathF2: -> DeathF2// Go to death 2 
 - isEnd: // Go to end
 - else: Error
 }
 
 =intro
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
    "I'm sure you'll remember soon enough. Let's just say you're quite special to me, and I'm quite special to you." ->whathappened
   
 * ["Hello?"]
 ~ CharacterTitle = "????????????"
    "Oh, thank the heavens. I was so worried that, in all the tumult, you might have forgotten who I was."
    
    "But I guess that's how peonies are–just when you think they've wilted completely, they suddenly blossom brighter than ever before."
   ~ CharacterTitle = ""
   <i>You feel as if you know this person, but struggle to even recall her name.</i> ->whathappened
   
   =whathappened
   *["What happened?"]
   ~ CharacterTitle = "????????????"
   "Hmm...I guess there is no easy way to say this." 
   ~ CharacterTitle = ""
   <i>The woman's face becomes pale. She fidgets with her necklace. </i>
   ~ CharacterTitle = "????????????"
   "You...passed on. Oh, you can only imagine how distraught I was to find your lifeless body out here. Thankfully, I was able to collect myself and begin preparations to bring you back." 
  ~ CharacterTitle = "Crypt Keeper"
  I am the keeper of this crypt, after all. It took a fair bit of work, yes, but you know I'd do anything for my peony." ->whathappenedcont
    
   =whathappenedcont
    
    *["How did you do such a thing?"]
     ~ CharacterTitle = "Crypt Keeper"
   "A magician never reveals her secrets...but since you're special, I'll make an exception." 
   
    "That lantern that hangs from your staff contains your soul. When in close proximity with it, your spirit can once again animate your body as it did while you were still...living."
   
    "I even carefully fitted your latern with some precious stones. If you're ever in danger, those gems will bring you back to me. I'm always here to take care of you, darling. When you feel the latern's glow, think of it as my embrace."
    ->lanterndesc
   
   =lanterndesc
   ~ CharacterTitle = ""
    <i>You look down at the lantern.</i>
     
    <i>Within it, a small candle burns soflty. The flicker of its flame is comforting. </i>
     
    <i>It reminds you of the broth you drank as a child when you were ill. </i>
     
    <i>The lantern itself is ornately sculpted. It's so polished that you can see yourself reflected in its silver casing. </i>
     
    <i>Someone clearly has put a lot of effort into preparing it for this occassion.</i> ->interrogation
     
    =interrogation
    *["Where is everyone?"]
    ~ CharacterTitle = "Crypt Keeper"
        "You don't remember? I'll tell you this much--I was right about that creature."
        
       "Radefell is gone. The survivors are living in this village. I've been helping them, here and there." 
        
        "I bet they'll be quite happy to see you. But please, let's not discuss this any further." 
        
        "Besides, you're already in such a sorry state. I can't imagine that dwelling on bad news is making you feel any better." ->interrogation
   
    *["Where am I?"]
    ~ CharacterTitle = "Crypt Keeper"
        "Apologies for the drab scenery. It's much easier to bring a soul back in a place such as this." 
        
        "Resuscitation is difficult in a city, where death is so omnnipresent."
        
        "But I guess it's an apt setting, seeing as I've cleaned you up like the mother dove cleans her young in the fountain's basin." ->interrogation
        
    *["Why revive me?"]
      ~ CharacterTitle = ""
      <i>She frowns. </i>
        ~ CharacterTitle = "Crypt Keeper"
        "Why revive anyone else? You're the lantern of my life, peony. You should know that better than anyone else." ->interrogation
    
    *["How did I die?"]
      ~ CharacterTitle = "Crypt Keeper"
        "I found you not far from here, in a nearby cornfield. You were badly bruised, especially on your knees and elbows."
       
       "It was a terrible scene, just awful. I was quite distraught, but I managed to pull myself together."
       
      "'This won't do.', I told myself, 'This won't do.' So I picked you up--all of you, including the fingers you had lost--and put you back together." 
      
      "Don't worry, I didn't peek under your mask. I know you're awfully sensitive about that."  ->interrogation
       

    *{CHOICE_COUNT() == 0}->
~ CharacterTitle = "Crypt Keeper"
       "Now then, I'm afraid I must wish you farewell. But worry not, my darling, for we shall meet again very soon." 
       ~ CharacterTitle = ""
       <i>She smiles at you, and–for the first time in a long time–you feel safe. </i>
       
       <i>You were safe at the chapel, sure, but any bird is safe in a cage. This is different. </i>
       
       <i>The woman snaps her fingers and disappears, leaving nothing behind but a faint aroma of lavender.</i>

        -> END

= DeathF1
~ CharacterTitle = ""
<i>In an instant, you feel yourself being thrust back into the cold embrace of life.</i>

~ CharacterTitle = "Crypt Keeper"
"You’ve met with yet another terrible end, haven’t you?" 

"Not to worry. I’ll be here–as I always have been–to put your soul back in its place." 

"Just...promise me you won’t do something I can’t bring you back from." 

"Don’t let your hunger for redemption destroy what we still have."
~ CharacterTitle = ""
<i>Breathe. </i>

-> END

= hub
~ CharacterTitle = ""
{Clergy.Hub && Artifact1<0: <i>You return to the Crypt Keeper, finding her tending to the crypt’s flora.</i> -> Artifact1}

~ CharacterTitle = "Crypt Keeper"
Hello, dear. I'd love to talk–that is, if you don't have any pressing matters to attend to.
~ CharacterTitle = ""
 *["I have a question."]
 ~ CharacterTitle = "Crypt Keeper"
 What would you like to discuss, my peony? -> conversation
 
 *["Goodbye."]
 ~ CharacterTitle = "Crypt Keeper"
 Until next time, then. Don't keep me waiting. -> END
 
 =conversation
 ~ CharacterTitle = ""
+["Why do we need the artifacts?"]
~ CharacterTitle = "Crypt Keeper"
"Well, that wretched being wants the artifacts."

"Thus, we have two choices..."

"We can let it carry out its scheme..."

"or we can intervene."

"That creature already destroyed Radefell. We can't let it move any further towards its goal." ->conversation

+["What exactly are the dungeons?"]
~ CharacterTitle = "Crypt Keeper"
"When the Malignance emerged, it wrought havoc upon Radefell. But it wasn't just blindly destroying everything." 

"It was taking pieces of the city and pulling them underground. This created the dungeons you now traverse."

"As to why would it do such a thing? I'm not sure, but I reckon that we'll find out soon enough." ->conversation

+["How are you holding up?"]
~ CharacterTitle = "Crypt Keeper"
"How kind of you to worry about me. I'm doing well, all things considered."

"In a situation like this, one is bound to miss her creature comforts." 

"I miss my bed, and the food here is...of variable quality." 

"But I still have you, my peony. That's all I need." ->conversation

+["Goodbye."]
~ CharacterTitle = "Crypt Keeper"
 "Until next time, then. Don't keep me waiting." -> END

= DeathF2
~ CharacterTitle = "Crypt Keeper"
{ shuffle once:

-   "How disappointing..."
- 	"What a thrilling death. I can see you've had practice."
-   "Oh dear, you know how much it pains me to see you get hurt."
-   "Someone's been busy! Let's get you patched up."
-   "I wonder...is it possible for one to get accustomed to death?"
}
~ CharacterTitle = ""
*[<i>Breathe.</i>]

-> END

=Artifact1
~ CharacterTitle = ""
<i>She seems to not acknowledge your presence. </i>

<i>Have you done something wrong? </i>

<i>Does she know about the High Priest returning? </i>

<i>There's only one way to find out.</i>

+[<i>Wait for her to acknowledge you</i>] -> wait
+[<i>Approach her</i>] -> approach 

=wait 
~ CharacterTitle = ""
<i>You observe her care for the plants for minutes on end.</i>
+[<i>Continue to wait</i>] -> continueWait 
+[<i>Approach her</i>] -> approach 

= continueWait
~ CharacterTitle = ""
<i>You stand idly for another minute, even though she has stopped watering the plants. </i>
<i>After the minute you hear her say...</i>
~ CharacterTitle = "Crypt Keeper"
"Well?"
->approach

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

+++["I'm ready to face the next challenge."] -> ready
+++["Perhaps some rest is in order"] -> rest
+++["..."] -> stare 

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
*["Do you feel different at all?"] -> different 
*["Do you know how the High Priest died?"] -> died 
*["What do you know of the three clerics?"] -> threeClerics 
*->HighPriestRise

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
->ClergyQuestions

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
+++[<i>Leave</i>] -> leave
+++[<i>Embrace her once more</i>] -> embrace 

=embrace 
~ CharacterTitle = ""
<i>You move in to embrace the Crypt Keeper once more. </i>

<i>She wraps her arms around you, tighter than the last. </i>

<i>Once again, she lets go, giving you a wave and a smile.</i>

-> END

=leave 
-> END

==Stick==
~ CharacterTitle = "Stick"
{
 - isHub: -> Hub // Go to hub
 - isDeathF2: -> Hub // Go to death 2 
 - else: Error ->END
 }
 
= Hub 
{StickHappiness:
 - 1: ->Stick1
 - 2: ->Stick2
 - 3: ->Stick3
 - 4: ->Stick4
 - 5: ->Stick4
 - -1: ->StickSad1
 - -2: ->StickSad2
 - -3: ->StickOHNO
 - -10: ->StickOHNO
 - else: ->NeutralStick
 }
 
// = DeathF2
// {
//  - FirstStickDeathF2 && StickHappiness>0: ->ConcernedStick
//  - else: ->Hub
//  }
// -> END

=NeutralStick
As you enter the Custodian's house, a melancholy dog approaches you.
Its collar reads: "Stick, my loyal and beloved pet"
<b>Stick:</b> *Whimper*
 *[Pet Stick]
    ~ StickHappiness = StickHappiness + 1
    You softly pet Stick's head
    <b>Stick:</b> Ruff, ruff!
    Stick happily wags its tail as you gently wave goodbye.
    ->DONE
 *[Leave Stick Alone]
    ~ StickHappiness = StickHappiness - 1
    As you turn and leave you can hear Stick whimpering softly.

-> END

=Stick1
Stick wags its tail happily as you enter.
+[Pet Stick again]
    {StickHappiness<2: 
        ~StickHappiness=StickHappiness+1
    }
    You softly pat Stick's head.
    <b>Stick:</b> Ruff, ruff!
    Stick happily wags its tail.
    ->PetStick
 *[Leave]
    ~ StickHappiness = StickHappiness - 1
    You ignore Stick and leave the house.
->END

=PetStick
 +[Pet Stick again]
    You softly pat Stick's head.
    <b>Stick:</b> Ruff, ruff!
    Stick happily wags its tail.
    ->PetStick
 *[Wave goodbye]
    You wave goodbye to Stick as you leave the house.
->END

=Stick2
You enter the Custodian's house. Little trinkets jingle on the wall.
At your presence, Stick perks up and its tail starts moving rapidly.
 +[Pet Stick again]
    {StickHappiness<3:
        ~StickHappiness=StickHappiness+1
    }
    You softly rub Stick's head.
    <b>Stick:</b> Arf!
    Stick nuzzles into your hand.
    ->PetStick2
 *[Leave]
    ~ StickHappiness = StickHappiness - 1
    You ignore Stick and leave the house.
->END
    
=PetStick2
+[Keep petting Stick]
    You softly rub Stick's head.
    <b>Stick:</b> Arf!
    Stick nuzzles into your hand.
    ->PetStick
 *[Wave goodbye]
    You wave goodbye to Stick as you leave the house.
->END

=Stick3
As you enter the Custodian's house, you see Stick get up and walk towards you, its tail swinging rapidly.
<b>Stick:</b> Ruff!
A bark of triumph.
 *[Pet Stick]
    {StickHappiness<4:
        ~StickHappiness=StickHappiness+1
    }
    You pat stick on the back.
    <b>Stick:</b> Woof!
    **["Good dog!"]
        <b>Stick:</b> Woof! Woof!
    **[Scratch Stick's back some more]
        <b>Stick:</b> Arf!
    - Stick's tail vigorously wags back and forth.
    You give Stick one last pet and unwillingly leave.
    <b>Stick:</b> Bark!
    ->DONE
 *[Leave]
    ~ StickHappiness = StickHappiness - 1
    <b>Stick:</b> Ruff! Ruff!
    You immediately turn around and leave, ignoring Stick's barks.
-> END

=Stick4
As soon as you open the door, Stick launches toward you.
You crouch down and recieve Stick in your arms.
 *[Pet Stick]
    {StickHappiness<5: 
        ~StickHappiness=StickHappiness+1
    }
    You give Stick a hearty back rub.
    <b>Stick:</b> Woof! Woof!
    ->PetStick3
*[Get up to leave]
    As you get up and start to leave, Stick follows.
    -> HappyLeave

=PetStick3
 +[Pet Stick again]
    You give Stick the best belly rub you've ever done in your life.
    <b>Stick:</b> Awooo!
    ->PetStick3
 +[Give Stick a treat]
    You find a jar of dog treats on a nearby shelf.
    You give Stick one of the treats.
    <b>Stick:</b> Ruff!
    Stick happily gobbles down the treat. 
    ->PetStick3
 +[Get Stick to do a trick]
    ->StickTrick
 *[Get up to leave]
    As you get up and start to leave, Stick follows.
    -> HappyLeave
 
 =StickTrick
 VAR trick = 0
 VAR trick2 = 0
 ~ trick = "{~1|2|3|4}"
 ~ trick2 = "{~1|2|3|4|5|6|7|8|9}"
 {trick:
    - 1: You grab a treat and spin it around Stick.
        Stick starts spinning in circles until it gets dizzy and has trouble standing up.
        You give Stick the treat.
        <b>Stick:</b> Ruh... Ruff...
    - 2: You grab a treat and tell Stick to sit.
        Stick sits down.
        You applaud and give Stick the treat. 
        <b>Stick:</b> Ruff!
    - 3: You grab a treat and ask Stick to {trick2==1: do a barrel roll|roll over}. 
        Stick ferociously rolls over and into the wall. 
        <b>Stick:</b> Woof! Ruff!
        Stick feels proud of their accomplishment.
        You feed stick the treat.
        <b>Stick:</b> Arf!
    - 4: As you go to grab a treat, you witness something amazing.
        Stick does a double flip into a 360 midair spin.
        Stick proudly lands on the floor and you give two treats for that.
        <b>Stick:</b> Arf! Arf!
    - else: ERROR RANDOM MODULE FAILED CONTACT YOUR NEAREST PELICAN
}
->PetStick3

=HappyLeave
However, the dungeon is too dangerous for a dog, so you stop in your tracks.
 * Tell Stick to stay
    You tell Stick to stay.
    <b>Stick:</b> *whine*
    Stick whines a bit but understands and backs off.
    With a heavy heart, you leave the Custodian's house.
 * Try to leave
    You try to leave, but Stick tries to leave with you.
    ->HappyLeave
 -->END
->END

=StickSad1
As you enter the Custodian's house, a heartbroken dog approaches you.
Its collar reads: "Stick, my loyal and beloved pet"
<b>Stick:</b> *Whimper* *Whimper*
 * Pet Stick
    ~ StickHappiness = StickHappiness + 1
    You softly pet Stick's head.
    <b>Stick:</b> Ruff, ruff!
    Stick happily wags its tail as you gently wave goodbye.
    ->DONE
 * Leave Stick Alone
    You once again turn and leave, ignoring the whimpers behind you.
->END

=StickSad2
You enter the Custodian's house looking around at the various shelves filled with dust.
Stick lays on the ground unresponsive to your intrusion.
    * Pet Stick
        You bend down and softly pet Stick's head.
        Stick remains unresponsive to you.
        ** Keep petting
            ~ StickHappiness = StickHappiness + 1
            You keep petting Stick.
            Eventually Stick perks up.
            <b>Stick:</b> Ruff...
            Stick slightly wags its tail as you gently wave goodbye.
            ->END
        ** Leave Stick Alone
            You leave, ignoring Stick alone on the floor.
            ->END
    * Leave the Custodian's House
        You leave, ignoring Stick alone on the floor.
        ->END
->END

=StickOHNO
You enter the Custodian's house with a creak. It's a dusty room with various trinkets and baubles.
You look around and find a dog bowl, several weird masks, and a few keys. However, you find nothing of importance here. 
-> WishfulThinking(StickHappiness)

=WishfulThinking(tries)
 + Keep looking
    You keep looking, but you don't find anything of interest.
    ->WishfulThinking(tries-1)
 * {tries == -8} Sit in silence
    ...
    ...
    ...
    ** ...
        ......
    ** ...
        ...............
    
    -- ~ StickHappiness = -10
    -> WishfulThinking(tries-1)
 * Leave
    You leave the Custodian's house, wondering why you ever even entered.
->END

-> END 
==Mayor==
~ CharacterTitle = "Mayor"
{
 - isMayorIntro: -> intro// Go to intro
 - isHub: -> Hub// Go to hub
 - else: Error
 }
 
 //INTRO CUTSCENE WHEN HE'S INTRODUCED OUTSIDE OF FLOOR 2
 
 =intro
As you arise from the dungeon, you notice a familiar man in a top hat, with rosy cheeks and thin, stitched lips. Golden strings hang limply from his limbs, like a discarded marionette. His smile brims with a melancholic mirth.

<b>MAYOR</b>: “...What a bitter state of affairs we find ourselves in."

+["You were there."] -> YouWereThere
+["I am sorry."] -> Sorry

=YouWereThere
The familiar man gives a tasteless, empty chuckle.

<b>MAYOR</b>: “Regrettably so, I fear. It brings some small joy that your memory hasn’t rotted like your form.”

<b>MAYOR</b>: “Still, I doubt I am the only one who harbors such remorse.”

-> questions

=Sorry 
The familiar man breathes deep through his nose, unbothered by the scent of death.

<b>MAYOR</b>: “You are not alone in fault, friend. Far from it.”

“Fear and complacency go hand in hand, after all.”

-> questions


= questions 
{questions < 1:  Recognition finally dredges memory of this man to the surface, one you had never spoken to but had seen so many times: from before the Convergence, the Mayor of Radefell, Poppet Meitar.}
<b>MAYOR</b>: “You must have questions. Ask away.”

+["How did you survive?"] -> survive 
+["What happened to the rest of the church?"] ->church
+["What are you doing here?"] ->here 
+["That's all for now."] -> EndOfIntro

=survive
<b>MAYOR</b>: “...I fled.”

Forlorn, the man’s posture visibly sinks.

<b>MAYOR</b>: “I hold no deeper shame. I knew that...thing...that something was coming, and I turned my back on those I purported service to.”

<b>MAYOR</b>: “Atonement may never grace me for such deep cowardice.”

-> questions

=church 
{church < 1: <b>MAYOR</b>: “...Most are gone. Just like everything else.”}

{church < 1: <b>MAYOR</b>: “Perhaps the only just perdition to have come from the Convergence after all this time.” }
*["Do you know who I am?"] -> whoIam
*["Couldn’t you have stopped what happened?"] ->stopped 
*-> questions

= here 
<b>MAYOR</b>: “Much has come about since the Convergence.” 

<b>MAYOR</b>: “By some twist of fate, the town has entrusted me with leading them once more. It remains to be seen if I deserve such station...”

The Mayor directs a heavy glance through a nearby window.

<b>MAYOR</b>: “...or if there is any good that remains to be done.”
-> questions


=whoIam
<b>MAYOR</b>: “Yes, I do recognize you. You were quite the illustrious disciple then...”

After a moment of silence, the man leans in to regard your appearance in greater detail.

<b>MAYOR</b>: “Not many within our strain of history return to this sense-dulled place.”
->church

=stopped 
<b>MAYOR</b>: ”I do not know, perhaps there were courses of action.”
 
<b>MAYOR</b>: “The church exploited my station, leaving me nothing but a puppet to their dissolute cause. I only came to the truth until it was too late.“

A deep sigh escapes from the man, his lips packed into a timeless grimace.

<b>MAYOR</b>: “Still, those courses are unexplored, those actions untaken.”

-> church


= EndOfIntro 
<b>MAYOR</b>: “If you wish to speak to me further, you can find me in my home.”

The Mayor regards your appearance and posture. He nods with approval, and speaks as he turns away.

<b>MAYOR</b>: “You always possessed an acute poise. Perhaps such a trait will allow you to right these wrongs of ours.”
-> END

=Hub
As you enter the room, the Mayor looks to regard you.
<b>MAYOR</b>: “It is not every day a corpse comes to speak to me. How may I be of assistance?” -> HubQuestions

=HubQuestions

+[Show the Whispering Bell] -> Artifact1
+{Artifact2Intro} [Show the Apparition Gauntlet] -> Artifact2
+{hasMayorNote1}[Show note about friend] -> note1
+{hasMayorNote2}[Show note about enigmatic eye] -> note2
+[Enough for now] -> goodbye 

=Artifact1
<b>MAYOR</b>: “Now, this is most intriguing.”

A foreign air fills the Mayor at the sight of the Bell, his posture gaining some form at the sight of the artifact.

<b>MAYOR</b>: “That much explains the buzz I have seen wash over the townsfolk as of late.”

<b>MAYOR</b>: “You’ve earned my commendation, albeit I doubt it holds much value in these times.”

<b>MAYOR</b>: “No doubt, however, that such an achievement rightfully deserves recognition, even deathless as you are. "

<b>MAYOR</b>: "Spelunking into the ruins of Radefell with horrors stalking its corridors seems amongst the least of pleasant ventures.”

+["It could be better."] -> better 
+["It could be worse."] -> worse 
+["It is unpleasant, indeed."] -> unpleasent 

= better 
A blanched laugh breaches Mayor’s rosy cheeks, his eyes slitting at the long unfamiliar sensation.

<b>MAYOR</b>: “Haah, ah...it always could be. That, in a way, is why you seek these artifacts.”

<b>MAYOR</b>: “Each step you take is toward the untwisting of our reality. Each artifact you reclaim is another leap toward the ending of these senseless times.”

<b>MAYOR</b>: “Hold to that, and you will see our wrongs righted.”

-> Artifact2Intro 

=worse 
A breathless chuckle escapes from the Mayor’s chest, a genuine, slight opening in his smile showing teeth.

<b>MAYOR</b>: “Heh, heh, that is a decent disposition to possess. Perhaps it will lighten you enough to lighten your step.”

<b>MAYOR</b>: “Nevertheless, hold to that optimism, my friend. It will be as effective a tool as any artifact, if you allow it so to be.”

->Artifact2Intro 

= unpleasent
The Mayor expels the humored thought through his nose, one half of his lip curling into a true smile.

<b>MAYOR</b>: “Then further respect for your efforts. I do not doubt that the work capable of redeeming ourselves or our world would ever be easy.”

<b>MAYOR</b>: “Yet...if enduring such an endeavor will cleanse the Malignance, then it will be well worth undertaking. You have already made great strides along this path, and such strides show that you are capable of much more.”

<b>MAYOR</b>: “Hold to the course, and I do not doubt that you will see it through.”

-> Artifact2Intro

=note1
<b>MAYOR</b>: “...Let me read it.”

You hold the letter out to the Mayor, whose hands tremble as his eyes regard the letter.

<b>MAYOR</b>: “Where did you procure this? Within the dungeon?”

<b>MAYOR</b>: “That can only mean...”

The Mayor grits his savorless teeth, a dry frost settling his gaze low.

<b>MAYOR</b>: “...My thanks for bringing this to me, my friend.”

With a downward tilt of his head, the Mayor’s face is locked in shadow, a tremble still visible at the tip of his nose.

<b>MAYOR</b>: “Please, forgive my upset. It is simply just...”

The Mayor’s eyes seem to weaken as he holds the letter out. You take it from his hand, and his arm folds close to his chest.

<b>MAYOR</b>: “The implication of such a discovery is another pain I will bear.”

<b>MAYOR</b>: “I had hoped when I did not see him or his family here, that he was swifter than I.” -> letter1Questions 

=letter1Questions

+["Who was he?]" -> whoWasHe
+["Why did you warn him?"] -> WarnHim
+["What now?"] -> WhatNow

=whoWasHe 
A distant light twinkles in the Mayor’s eye, and youthful air fills his chest.

<b>MAYOR</b>: “A friend whom I once loved as a brother.”

<b>MAYOR</b>: “If I could turn back time,”

<b>MAYOR</b>: “I would have never let such kinship fade.”

->letter1Questions

=WarnHim 
<b>MAYOR</b>: “On that fateful day, I finally broke the Church’s leash. Station and position carries little value in the face of death.”

<b>MAYOR</b>: “It was too late, however, to speak out against the false truth the Church peddled. Radefell was too fervent in the belief that its salvation lay in the bringing of what we now know as the Malignance.”

<b>MAYOR</b>: “I only hoped that one would heed me, for respect of all those years past.”

->letter1Questions

=WhatNow 
The Mayor inhales through his nose as he reforms his posture.

<b>MAYOR</b>: “There will forever be things we will wish to change from our pasts, yet the truth of the past can never be altered.”

<b>MAYOR</b>: “All that remains is to seize the present, and to strive toward a future we can be proud to call our past.”

<b>MAYOR</b>: “That is why I have chosen to remain. Since my history is marked with inaction, I will gift any mote of good I can to the future of Radefell.”

<b>MAYOR</b>: “Nevertheless, Is there anything you require?”

-> HubQuestions

=note2 
<b>MAYOR</b>: “Allow me, please.”

You hand the Mayor the page. His eyes squint and his lips frown in recognition.

<b>MAYOR</b>: “Indeed, yes. This is my penmanship. I was witness to one of the artifacts, before the Convergence.”

<b>MAYOR</b>: “What do you wish to know?”

-> Letter2Questions 

=Letter2Questions 
+["What is this artifact?"] -> artifactQuestion
+["When did you see it?"] -> seeIt 
* -> HubQuestions

=artifactQuestion
<b>MAYOR</b>: “The retainer who demonstrated the artifact called it the Eye of Genesis.”

<b>MAYOR</b>: “They claimed it would unite the powers of the artifacts, and bring form to the Malignance.”

<b>MAYOR</b>: “I cannot comprehend the extent of its power, only that it possesses it.”

<b>MAYOR</b>: “And its gaze...I hesitate to match its gaze again.”

->Letter2Questions

=seeIt 
<b>MAYOR</b>: “The morning of the Festival, hours before the Convergence. As the menace of the artifact washed over me, the revelation of horror that would befall us would come too late.”

<b>MAYOR</b>: “All of Radefell looked toward the Church as their saviors, and none heeded my warning as I raced through the streets.”

->Letter2Questions

= Artifact2Intro
<b>MAYOR</b>: “Nevertheless, there is still much work for myself to do. No doubt the same for you, with other artifacts to collect.”

->questions

= Artifact2 
You see surprise tug the lids of the Mayor’s eyes open, and his stitched lips file into an impressed line.

<b>MAYOR</b>:“Now this is...most auspicious.”

The Mayor’s sight seems to settle on something that isn’t you for a moment, his next words half-spoken as his mind drifts.

<b>MAYOR</b>: “No doubt such diligence deserves recognition.”

+["Glad to be of service."] ->service 
+["I deserve no thanks."] -> thanks 
+["It is my duty."] -> duty
+["The work remains unfinished."] -> unfinished 

=service 
The Mayor’s gaze focuses on the wall behind you for a few seconds, until he looks back to you, coming to his senses.

<b>MAYOR</b>: “Service is a vast understatement.”

->HubQuestions

=thanks
The Mayor’s gaze turns downward for a few seconds, until he looks back to you, coming to his senses.

<b>MAYOR</b>: “If you believe it to be so, then I will not impose my gratitude upon you. Just know that the course you took has brought bounty after the most brutal of famines.”

->HubQuestions

=duty 
The Mayor’s gaze lies upon a way to his right for a few seconds, until he looks back to you, coming to his senses.

<b>MAYOR</b>: “That I cannot fault, even risen as you are. You have chosen it to be so nonetheless, and you have performed such a task with excellence.”

->HubQuestions

=unfinished

The Mayor’s gaze seems unfocused for a few seconds, until he coughs and looks back to you, coming back to his senses.

<b>MAYOR</b>: “Quite, yes, yet there is still impact from what you have already done. Pride can be claimed in that.”

->HubQuestions

= goodbye
<b>MAYOR</b>: “I wish you well in your endeavors.”
-> DONE
==Clergy==
~ CharacterTitle = "High Priest"
{
 - isIntro: -> intro // Go to intro
 - isDeathF1: -> DeathF1 // Go to death 1
 - isHub: -> Hub// Go to hub
 - isDeathF2: -> DeathF2// Go to death 2 
 - else: Error ->END
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
<i>The condescension drips sickly sour off of their lips as their eyes alight upon you. The grand cathedral around them seems to mock you, too, as their words reverberate off the walls. You can almost imagine the sound of a chuckle from the dark throne cloaked in shadow at the opposite end of the vaulted hall.
 *  "I have no need for holy light, but I could use some answers."
    The thoughtful cleric spreads their arms wide.
    ~ CharacterTitle = "Thinking"
    "We have little to hide. As long as you remain diplomatic, we will answer any questions you possess."
        -> AskQuestions
 *  "Let me pass, petulant ones. I care not for your distractions."
    -> GoForth2

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
PLAYER SUCCEEDS IN FLOOR 1 SECTION (?? will the other things take precedence over hub? hopefully hub will only ever be triggered once)
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
<b>HIGH PRIEST</b>: "Your transgressions mark a black stain upon the lightless void. Identify yourself, heretic, among the three: are you traitor, puppet, or monster?"
~ CharacterTitle = ""
 *  "I am no traitor."
 *  "I am no one's puppet."
 *  "I am not a monster."
 ~ CharacterTitle = "High Priest"
-<b>HIGH PRIEST</b>: "Maybe not, but you are weak. Though you have stolen from my dungeon, you have only shown the futility of your quest."
<b>HIGH PRIEST</b>: "My children grow hungrier. Your final death is imminent."
<b>HIGH PRIEST</b>: "Now, get out of my sight."
    -> END


 /*
PLAYER DIES IN FLOOR 2 SECTION
*/

= DeathF2 
~ CharacterTitle = ""
<i>The menacing figure of the High Priest turns to you, the other clerics cowering behind.</i>
 ~ CharacterTitle = "High Priest"
<b>HIGH PRIEST</b>: "My clerics forewarned me of the great humor of your repeated failings, but nothing could have prepared me for the reality of your humiliation."
~ CharacterTitle = ""
  *  "Enough mocking, I have questions."
    <i>The High Priest's black eyes burn twin holes in your head, but they do not decline your request.</i>
        -> AskQuestionsOfHP
 *  "I will not speak to you any more than I must, fear-father. Let me pass."
    -> GoForth3

= AskQuestionsOfHP 
~ CharacterTitle = ""
 *  "What are you?"
    -> WhatAreYou
 *  "What do you think of the villagers?"
    -> WhatOfVillagers
 *  "You returned when I discovered the Magic Hand, why? What do you want?"
    -> WhatYouWant
 *  "Enough. I wish to pass on into the dungeon."
    -> GoForth3

= WhatAreYou 
 ~ CharacterTitle = "High Priest"
<b>HIGH PRIEST</b>: "I am a vision of myself, wrenched from my infinite form into this weak body."
~ CharacterTitle = ""
<i>Chills creep up the back of your neck. The High Priest's cold stare holds you, immobilized.</i>
 ~ CharacterTitle = "High Priest"
<b>HIGH PRIEST</b>: "A better question, I ask myself: what are you?"
<b>HIGH PRIEST</b>: "Death-cheater, oath-breaker, disgraced. Despite your resistance, you are destined to be nothing, just as I am destined to be everything."
<b>HIGH PRIEST</b>: "Fate will not relent for you, and time will reveal my true nature."
- -> AskQuestionsOfHP

= WhatOfVillagers 
 ~ CharacterTitle = "High Priest"
<b>HIGH PRIEST</b>: "They are, all but one, wretched invertibrates. Which of them interests you?"
~ CharacterTitle = ""
 *  "The Crypt Keeper."
  ~ CharacterTitle = "High Priest"
    <b>HIGH PRIEST</b>: "She is a betrayer, heathen, and the most egregious of failures, despite her talents."
 *  "The Mayor."
  ~ CharacterTitle = "High Priest"
    <b>HIGH PRIEST</b>: "He is a weak-willed puppet, and it was delicious fun to toy with him before he fled during my rise."
 *   "The Scholar."
  ~ CharacterTitle = "High Priest"
    <b>HIGH PRIEST</b>: "They are a poor, tortured monster, and I cannot help but adore their fervor. They are also quite a talented keeper of secrets, to be sure."
- -> AskQuestionsOfHP

= WhatYouWant 
 ~ CharacterTitle = "High Priest"
<b>HIGH PRIEST</b>: "You've brushed so close against success..."
<b>HIGH PRIEST</b>: "I want to bear witness to when you inevitably fail."
-> AskQuestionsOfHP

= GoForth3 
 ~ CharacterTitle = "High Priest"
<b>HIGH PRIEST</b>: "Of course... I will see you again shortly, doomed heretic."
    -> END
==Scholar==
 
 =Hub
 ~ CharacterTitle = "Scholar"
 {
- Hub == 0: -> ScholarhubIntro
- else: -> regularHub
} 


=ScholarhubIntro
Despite your misgivings, you walk towards the uninviting building. 

As you approach, the door creaks open, and a robed figure appears from the deep shadows of the doorway. 

The little of their face left uncovered by bandages sends a shiver down your spine. 

And yet...a wave of familiarity crashes upon you all the same. 

The gravelly voice from the person before you echoes louder in your head than in your ears.

<b>SCHOLAR:</b> Hmm, so it appears one has returned...
It has been a long while since I’ve conversed with someone... of a similar origin. 

<b>SCHOLAR:</b> It’s good to know we few are not the last.

*["What do you mean?"] ->WhatDoYouMean 
*["It is good to see you too."] ->GoodToSee

=WhatDoYouMean 
<b>SCHOLAR:</b> Hmm? Has the wanderer lost themself in their years away? Surely, they must recognize one of their Church’s great knowledge keepers.

<b>SCHOLAR:</b> Not much for talk, wanderer? I can’t say I’m surprised. The eternal enigma they are, even to one as learned as I. 

-> introQuestions 

=GoodToSee 
You give a deep bow. 

<b>SCHOLAR:</b> Ahh, such respect from the wanderer.

<b>SCHOLAR:</b> Nothing less than I’d expect from Malisense’s most faithful.

<b>SCHOLAR:</b> It seems, even after all this time, they remember how to respect their superiors.

->introQuestions

=introQuestions
<b>SCHOLAR:</b> But, niceties and remembrances aside, I doubt idle prattle is what the wanderer seeks. What is it they wish of me?

*["What happened to you?"] -> toYou 
*["What happened here?"] -> happenedHere 
*-> introGoodbye 

=toYou 
<b>SCHOLAR:</b> Isn't that an interesting quandary. 
<b>SCHOLAR:</b> And the answer is even more intriguing.
<b>SCHOLAR:</b> But, forgotten wanderer, I hardly wish to reveal such knowledge to one who has become practically a stranger. 
<b>SCHOLAR:</b> Perhaps some other time I shall tell, but for now the doors to my secrets are locked.

->introQuestions

=happenedHere 
<b>SCHOLAR:</b> I take it the wanderer fled before the damned city fell? 

<b>SCHOLAR:</b> Well, in essence, after the Ritual that day, the great evil that once laid beyond our limited mortal grasp finally gained physical form, and with that form decided to drag humanity down to the damnation it felt we deserved. 

<b>SCHOLAR:</b> In an instant, the city disappeared beneath the Earth, and those who remained either lost their minds or their freedom to the Malignance. 

<b>SCHOLAR:</b> Evidently, some people were able to escape the destruction–though none unscathed–and decided to go about the process of rebuilding...or whatever the closest thing might be. 

<b>SCHOLAR:</b> I doubt the Radefell we knew shall ever stand as it did before...

-> introGoodbye 

=introGoodbye
They let out a heavy sigh.

<b>SCHOLAR:</b> It has been so long since I have spoken so much to another. 

<b>SCHOLAR:</b> But, I assume the wanderer has something somewhere to attend to. So, leave, then. 

<b>SCHOLAR:</b> Come again with any items of interest, and I shall tell of their nature. 

<b>SCHOLAR:</b> And...give my regards to the Crypt Keeper. If she's still around.
-> END

=regularHub
    As you approach the stout building once more, the dimly-lit windows signal to you that the Scholar is at home.
    *[Knock on the door]
    -> ArtifactTibits
    
= ArtifactTibits 
{ArtifactTibits < 1 : <b>SCHOLAR</b>: The wanderer is eager to know more? Please, enter my domicile and pilfer away at my wealth of knowledge. }

{ArtifactTibits < 1 : <b>SCHOLAR</b>: Let us fill your mind with forgeries. }

{ArtifactTibits < 1 : <b>SCHOLAR</b>: I know the wanderer seeks to know more of the artifacts...I shall share my burden with the wanderer so that it no longer crushes solely my shoulders. }

{ArtifactTibits < 1 : <b>SCHOLAR</b>: There are three. The Whispering Bell, the Apparition Gauntlet, and the Eye of Genesis. ->ArtifactQuestions} 

{ArtifactTibits < 2: <b>SCHOLAR</b>: Back already, hm? I am pleased the wanderer's steps have not ceased along their journey, and have instead brought them back to me.->ArtifactQuestions} 

{ArtifactTibits > 3: A faint memory emerges from the deep crevaces of your mind. You're sitting alone in an office.} 

{ArtifactTibits > 3: You must've been seven or eight years old. The Scholar helping you with your studies. }

{ArtifactTibits > 3: They've made little finger puppets, which they're now using to try and explain the Old Wars to you. Where did that cheery person go? Maybe they never left.}

{hasDied: <b>SCHOLAR</b>: Ah, what joy! The wanderer returns once again! I had worried our last meeting would be our final one. What a tragedy that would have been... ->ArtifactQuestions} 

=ArtifactQuestions 
<b>SCHOLAR</b>: What do you wish of me?

 *  "What is the Whispering Bell?"
    -> WhisperingBell
* {WhisperingBell} "May I show you something?" -> AfterA1

 *  "What is the Apparition Gauntlet?"
    -> ApparitionGauntlet
* {ApparitionGauntlet} "May I show you something?" -> AfterA2

 *  "What is the Eye of Genesis?"
    -> EyeofGenesis
    
 *  "I'm done here."
    -> END

*-> CharacterQs


= WhisperingBell 
<b>SCHOLAR</b>: According to legend, the Whispering Bell was formed deep underground–much like a diamond. 

<b>SCHOLAR</b>: Over time, the bell was pushed closer and closer to the surface, until it was eventually found by the great General Dymilos. 

<b>SCHOLAR</b>: Many historians attribute Dymilos' strategic prowess to the bell, which he claimed allowed him to see enemy soldiers through castle walls and country hillsides. 

<b>SCHOLAR</b>: The battles he won were the crux of Radefell's glory. Our city would not have held as long as it did without his strength. 

<b>SCHOLAR</b>: When he died, the bell was taken by the Church, as revered as a sacred item. 

<b>SCHOLAR</b>: Upon the death of a devotee, the bell would be rung once in honor of their life lived in sacrifice to the Church. 

<b>SCHOLAR</b>: The wanderer must remember its tolls, even if they knew not from where. And it seems the bell has yet to toll for them. 

<b>SCHOLAR</b>: Keep it that way, dear wanderer. 

-> ArtifactQuestions

= ApparitionGauntlet 
<b>SCHOLAR</b>: Indeed, the Apparition Gauntlet! An item of great power, and greater mystery. I admit, despite my position as Radefell's last great Scholar, even I have very little knowledge of its true origins. 

<b>SCHOLAR</b>: However, many legends agree that this artifact sprung from the earth, as if grasping for the light of the surface it knew only whispers of. 

<b>SCHOLAR</b>: Supposedly, it's outstretched grasp is fueled by the anger of the souls sacrificed to the Malignance, so much so that they can still influence the mortal world. 
-> ArtifactQuestions

= EyeofGenesis 
<b>SCHOLAR</b>: The Eye of Genesis...once a seemingly mundane pendant, it was adorned with a curious eyeball. 

<b>SCHOLAR</b>: This was originally utilized by the Church as a way to oversee everyone within it. Some say its ever watchful gaze is constantly predicting the next movements of all within its line of sight. 

<b>SCHOLAR</b>: As for how it tells the clergy about misbehavior or imminent issues, that is a closely guarded secret of the Church that only the High Priest and few others knew. 

-> ArtifactQuestions

= AfterA1 

<b>SCHOLAR</b>: Ahh, it seems our intrepid wanderer has brought back something interesting. Brilliant, brilliant. 

<b>SCHOLAR</b>: I hope the wanderer does not mind my writing as we talk. My mind has been clouded since Radefell perished, and nearly I along with it. 

<b>SCHOLAR</b>: Writing helps me remember...remember what I am, what it was, and who we are. 

<b>SCHOLAR</b>: Now, what is it that the wanderer wishes to show me?

You show them the bell. 

Instantly, the Scholar's remaining eye lights up. 

You've never seen them in such a state. 

Their excitement makes them seem almost monstrous, their hand quickly scribbling unrecognized symbols on their parchment. 

<b>SCHOLAR</b>: My, my, my, what have we here? Such an incredible item they seem to have brought to show me! 

<b>SCHOLAR</b>: To the wanderer's layman eyes, I imagine it appears to simply be a bell. 

<b>SCHOLAR</b>: What was their clerical title, again? Well, no matter, they have earned my knowledge of this artifact. 

* "Please, tell me more."
 -> tellMore
 
* "Just get on with it, already."
 -> getOn

= tellMore  
<b>SCHOLAR</b>: It would be my greatest pleasure.

You hold the mysterious bell up to the Scholar. 

Though their expression remains concealed, you can feel the giddiness rising within them as they hungrily take in every inch of the artifact. 

<b>SCHOLAR</b>: Ah, yes. Yes! What serendipity! The wanderer has brought to me the Whispering Bell!
-> WhisperingBellExplination

= getOn 
<b>SCHOLAR</b>: Such impatience. The wanderer must learn to wait... 

 They begin eyeing the bell thoroughly. Though their expression remains concealed, you can feel the giddiness rising within them as they hungrily take in every inch of the artifact.. 

<b>SCHOLAR</b>: Certainly this cannot be...Yet, it is...wanderer. Oh, gracious wanderer! You have brought to me the Whispering Bell! 

-> WhisperingBellExplination

= WhisperingBellExplination
<b>SCHOLAR</b>: Mortal minds cannot hear the sound of the bell. If I could, I shudder to think of how terrible it might sound. 

<b>SCHOLAR</b>: Seeing it here, in the flesh, I can confirm a few suspicions that it would have been unbecoming of me to voice before. 

<b>SCHOLAR</b>: There were rumors it was used to track signs of the Malignance when rung, and it thusly must allow you to track the souls corrupted by it. As for a reward for showing me this, I shall tell you of my folly, and of the knowledge I have burdened myself with.

*["Your folly?"] -> folly 

=folly 
<b>SCHOLAR</b>: I seek memories of the Convergence, to preserve my own humanity, as I now have the unfortunate position of feeling empathy for the Malignance, yet at least I hold my own memories. 

<b>SCHOLAR</b>: I lost my peers, and my own control over my body, and all for what? 

<b>SCHOLAR</b>: To have no mouth with which to speak my knowledge, to bear the burden of these secrets alone...at least I may tell you without any fear. That is all I will say for now. 

<b>SCHOLAR</b>: Now, go and wander once more, down in the dark and despair of the Malignance's tomb. 

<b>SCHOLAR</b>: The ringing echoes of the dead shall clear the way of the corrupted.

-> END

= AfterA2 

<b>SCHOLAR</b>: Brilliant, simply brilliant. The wanderer never fails to impress, as I should have expected. It seems even I have forgotten why they were so close to the heart of our Church... 

You hold up the Apparition Gauntlet.

As the gilded hand shimmers in the faint light before the doorway, you see its gleam reflected in the Scholar's eyes. Their excitement is apparent. 

<b>SCHOLAR</b>: It...This is...wanderer. My dear wanderer! What blessings they bring to my doorstep, for they have found me the Apparition Gauntlet!

<b>SCHOLAR</b>: To think that such a legendary artifact is here before me now. 

 * "Please, tell me more."
 -> tellMoreGautlet 
 
* "Just get on with it, already."
 -> getOnGautlet
 
 =getOnGautlet 
 The Scholar's brow furrows slightly.
 
 <b>SCHOLAR</b>: It seems I have also forgotten their lack of patience. But, no matter. Bring it here.
 
 ->tellMoreGautlet
 
 = tellMoreGautlet 
{-getOnGautlet: 
<b>SCHOLAR</b>: But, no matter. Bring it here.
-else:
<b>SCHOLAR</b>: If you'll allow me...
}

The Scholar reaches over to touch the Gauntlet, then shudders. 

<b>SCHOLAR</b>: It is as I thought, my intuitions were correct...

-> handQuestions

=handQuestions

*["How should I use it?"] -> howUseHand 
*["What was its use?"] -> useHand
*->handGoodbye

=howUseHand 
<b>SCHOLAR</b>: It seems the souls consumed by the Malignance still claw for the surface of a tempestuous sea of their own multitudes. 

<b>SCHOLAR</b>: Their outstretched grasp and its lack of a grip on reality ends up overcompensating, so that its infrequent touch far outstretches the physical bounds of the Gauntlet's fingers. 

<b>SCHOLAR</b>: For those with a vocabulary lesser than mine, it may touch others from very far away in its attempts to cling to the mortal realm.

-> handQuestions



=useHand

<b>SCHOLAR</b>: This forces me into a position of conjecture given how much of the information about it is knowledge or rumor, but considering the truth of it being the consumed souls of the Malignance vying for revenge...

<b>SCHOLAR</b>: It must have been used by the Church for the controlling of beasts...still, as thanks for sharing such a wonderous creation with me, I shall speak more of my senselessness...

<b>SCHOLAR</b>: If I had merely left the clergy like the Crypt Keeper, my body would not be in such a state as this...oh, what a fool I've been. 

*["Your body?"] 

<b>SCHOLAR</b>: I...do not know if it is safe to share this secret but...my body is not my own. 

<b>SCHOLAR</b>: I only make use of the eye and my mind. I am left as a puppet, much like the Mayor. 

<b>SCHOLAR</b>: N-Not in the political sense or whatever is going on with that coward. 

<b>SCHOLAR</b>: It was a great mistake of mine, to give up my body to a beast that controls it...I am grateful you are the only visitor I get. 

<b>SCHOLAR</b>: I shall speak no more of it to you....
-> handQuestions

=handGoodbye 
<b>SCHOLAR</b>: Now, go and wander once more, down in the dark and despair of the Malignance's tomb. 
->END 

= CharacterQs 
* "What do you know about the Crypt Keeper?"
-> ckAnswers

* "What can you tell me about the Mayor?"
-> mayorAnswers

* "Anything to say about the clergy?"
-> clergyAnswers

* "Enough questions."
-> END

= ckAnswers 
<b>SCHOLAR</b>: So you wish to know more about the Crypt Keeper...I must say, there is some knowledge I am not burdened by. 

<b>SCHOLAR</b>: I know not if it is my place to say this, but rightfully she left the clergy long ago while I remained, blinded by my search for knowledge. 

 "Why did she leave?" 

<b>SCHOLAR</b>: There were times where the good she could do was limited by being in the Church. And so she left, for a better life. And look where it got her in comparison. 

 They reach to touch the bandages on their face, then shudder.  

<b>SCHOLAR</b>: T-That is enough. Get on with any other questions you have. But please, send the Crypt Keeper my regards. She did the right thing.

-> CharacterQs

= mayorAnswers 
<b>SCHOLAR</b>: The world of politics was rather out of my purview of knowledge, I know not the intricacies of negotiation and I most certainly never have known the intricacies of fleeing like a coward. 

<b>SCHOLAR</b>: I have done many things that were senseless, but never as senseless as that fool of a mayor. 

 "Senseless?" 

<b>SCHOLAR</b>: The wanderer seeks to know of my many mistakes, but I shall only speak of it if you show me any and all artifacts you collect. 

-> CharacterQs
= clergyAnswers 
<b>SCHOLAR</b>: The clergy...all I know is that they are members of the Church, with the exception of the High Priest...they kept me on a short leash, treating me like a curious infant, even when I was one of the foremost keepers of Church secrets. 

<b>SCHOLAR</b>: Perhaps they were right that I would never learn some sort of higher truth, and I never left the clergy like the Crypt Keeper did...if I had, maybe I'd have learned some greater truth.

-> CharacterQs




==End== 
~character = "Mayor"
~ CharacterTitle = "Mayor"
~background = "next"
You burst into the Mayor’s house, gripping the note in a fierce clutch. You betray your lightfooted step and close the distance across the room in an instant, locking eyes with the Mayor. 

While he does not flinch from your gaze, the Mayor’s lips are pressed together, but not smiling this time.	

<b>MAYOR</b>: “I knew this time would come...”

+["You have the final artifact!"]  

<b>MAYOR</b>: “Indeed. It is within my possession, and I am willing to tell you where it is.”  

++["Tell me."] 

“I hid it within the fountain, at the center of town. You can find it there.”

+++["Tell me what happened."] -> Explination 
+++["I am done with you."] -> CryptKeeperFinal

= Explination
{Mayor.note1: <b>MAYOR</b>: “You know the story, you found that scrap of my journal. Allow me to illuminate the full context.”}

<b>MAYOR</b>: “Even as I acted toward the supposed good of Radefell, I was but an unknowing puppet for the Church, unaware of what would come until it was too late.”

<b>MAYOR</b>: “The evening before the Convergence, the Church channeled the gall to demonstrate the Eye of Genesis to me. They must have known that not a soul would heed me, that I would tarnish my image in my doomsaying and they would rise to establish their theocracy over Radefell.”

<b>MAYOR</b>: “We know now that it would have never been that simple.”

+["What happened before the Convergence?"] -> PreConvergence
+["I am done with you."] -> CryptKeeperFinal

=PreConvergence
<b>MAYOR</b>: “I toiled, unable to come to a course of action that could stop the Church. I am no warrior nor sneak, so I had no chance of infiltrating where they kept the Eye of Genesis.”

<b>MAYOR</b>: “So...I simply waited. Listening to the final celebrations of many lives. Even though I foresaw the apocalypse that would unfold, I could not predict the specificity of its horror.”

<b>MAYOR</b>: “As the Malignance began to break through and across our reality, I ran into the Church’s doors, wrote the note, and ran like fire with the Eye, hoping that I or another would discover a method for aiding against the Malignance in the future.”

<b>MAYOR</b>: “I passed those screaming to hear their own voices, so many toppled over, locked in a hell within their body. Unable to move, unable to see, losing the ability to know what horror had befallen us.”

<b>MAYOR</b>: “In that flight from Radefell, the chance to aid those who I warned laid before me, and I left them to their fates.”

<b>MAYOR</b>: “I damn myself for each and every soul I abandoned that night.”

+["There was nothing you could do."] -> NothingToDo
+["I am done with you."] -> CryptKeeperFinal

=NothingToDo
The Mayor’s gaze flits low, his dry teeth clenching nearly hard enough to crack.

<b>MAYOR</b>: “I cannot accept any excuse for my cowardice, my friend. It is my failure, and mine alone. I will give myself to whatever good I can accomplish, but I know it will never pay the full tithe of my sin.”

<b>MAYOR</b>: “Especially after this final negligence...”

+["Deservedly so."] -> Deserved
+["Leave the past behind."] -> behind
+["I am done with you."] -> CryptKeeperFinal

=Deserved
The Mayor vents a tasteless breath, a pained smile plastered upon his face.

<b>MAYOR</b>: “Indeed, my friend.”

+["Why didn’t you tell me where the Eye is?"] ->WhyDidntYouTellMe
+["I am done with you."] -> CryptKeeperFinal

=behind 
The Mayor smiles, the thin stitch of his lip trembles as his eyes seem to vibrate in his skull.

<b>MAYOR</b>: “I...I do not know if I can, my friend. The faces, lost and twisted with the horror of their very existence deprived from them, they haunt me every time I close my eyes.”

<b>MAYOR</b>: “Even further, there is one final negligence of mine...”

+["Why didn’t you tell me where the Eye is?"] ->WhyDidntYouTellMe
+["I am done with you."] -> CryptKeeperFinal

=WhyDidntYouTellMe
<b>MAYOR</b>: “Knowing your task of collecting the artifacts, I realized that my displacement of the Eye of Genesis would need to be revealed.”

<b>MAYOR</b>: “I did not yet know your capability when you arose, and there was the potential outcome of you losing it in the depths of Radefell where it would be out of reach. I kept it secret then, and I would reveal it to you once you had returned with the second artifact.”

<b>MAYOR</b>: “But then...hope came to the village. I enjoyed working with our fellows, and a distant contentment finally came to me.”

<b>MAYOR</b>: “I thought that with the unveiling of the Eye’s location and the cleansing of the Malignance, it would come time to bring those who allowed the Malignance’s proliferation to trial.”

<b>MAYOR</b>: “So, instead, I opted not to inform you. I could enjoy the little, happy life here until it would be time to come clean.”

<b>MAYOR</b>: “And that time has come. I won’t pretend that such actions were not selfish.”

->Selfish

=Selfish
*["Why do it then?"] -> WhyDoIt
*{WhyDoIt}["You deserve to be punished."] -> punish
*{WhyDoIt}["I would have protected you."] -> protected
*{punish} or {protected} ["There is one thing I wish you to do."] -> Wish
+["I am done with you."] -> CryptKeeperFinal

=WhyDoIt
The Mayor’s rosy cheeks reach a high blush, blended deep with shame.

<b>MAYOR</b>: “Perhaps, we can never escape our true nature, no matter how much we wish to change.” 

->Selfish

= punish

<b>MAYOR</b>: “I don’t disagree, I’m afraid.”

<b>MAYOR</b>: “I will accept whatever judgment comes to me. I will give whatever remains of my word to that.” 

-> Selfish

= protected
<b>MAYOR</b>: “That is a boundless kindness. One that I could never have predicted.”

<b>MAYOR</b>: “One I would not deserve even had I revealed the Eye sooner.”

-> Selfish

=Wish
The Mayor nods, awaiting your word.

+["Await your judgment here."] -> Judgement
+["Life free of the past."] -> Past
+["Submit to condemnation."] -> Condemnation
+["Give yourself to the future of Radefell."] -> GiveYourself

= Judgement 

<b>MAYOR</b>: “Very well. I will do as you ask. Farewell, Disgraced.”

<b>MAYOR</b>: “End this curse, once and for all.”

->CryptKeeperFinal

= Past
<b>MAYOR</b>: “I...I will do my best to, my friend.”

<b>MAYOR</b>: “Go. End this curse, once and for all.”

->CryptKeeperFinal

= Condemnation
The Mayor simply nods, then bows his head, closing his eyes as he whispers his last words.

<b>MAYOR</b>: “Go. End this curse, once and for all.”

-> CryptKeeperFinal

=GiveYourself
The Mayor’s stitched lip curls inward, but he nods.

<b>MAYOR</b>: “Very well, I can hold true to that. Farewell, Disgraced.”

<b>MAYOR</b>: “Go. End this curse, once and for all.”

-> CryptKeeperFinal


=CryptKeeperFinal
<b>MAYOR</b>: “Farewell then, Disgraced. End this curse, once and for all.”
~background = "next"

You approach the fountain, discerning it closely for the artifact. 

It isn't until you look closely into its waters that you spy a metal box, barely off-color with the stone, at the bottom of the fountain. 

You bend and reach in, heaving the box out of the water, set it down, and open its lid.

Within the box lies The Eye of Genesis. 

Its gaze almost seems to apprise you as you take it into your hands. 

~character = "Crypt_Keeper"
~ CharacterTitle = "Crypt Keeper"

You hurry back to meet with the Crypt Keeper, the Eye of Genesis, the final artifact, in hand. 

As you approach her, she remains silent, unmoving. Instead, she focuses on the barren land surrounding the town.

*Get closer
    Her acknowledgement of your presence is betrayed only by tinge of sorrow that has appeared in her face.
    **Sit down next to her
        "You know what this means, right?"
        ***"Of course."
             "Yes, yes...You know what this means for the Malignance, you know what this means for Radafell..."
             
                Her tone of voice raising with every passing word.
                ->WhatItMeans
        ***"I do not."
            ->WhatItMeans
        ***"..."
            ->WhatItMeans

==WhatItMeans
She takes up your hands, and finally makes eye contact with you. Her eyes water.

"Do you know what this means for you?"
*"For me?"
    "Yes! For you! You haven't sat down and thought about what all this means for you?"
    
    Her grip on your hands strengthen, averting her eyes once again.
    **"I have not."
        "Of course you haven't. Of course you haven't...and that's why you were the only person who could have succeeded in this quest."
        ***"What does this mean?"
            Any words that she tries to get out are choked up by her tears as she embraces you, burying herself into your chest. 
            
            ****Comfort her
               For a minute, she remains like this. You feel her shoulders heave up and down as she continues to silently weep with you in embrace.
               
                  She looks back at you. 
                 
                  "It...it's for the best though. Please, my peony, give me the artifact."
                 
                  ******Give her the artifact
                        After taking the artifact, she embraces you one final time. She pulls away before she begins the cleansing ritual of the Eye of Genesis.
                        
                        *******Breathe
                            As the process goes on you start to feel tired, weak, like you can't...
                            
                            ********Breathe
                                
                                The light fades. The only thing that you can envision are your memories.
                                
                                ->SensesReturn

==SensesReturn==
*Look
    Your adventures, the monsters you've encountered, the people you've met, the city you've saved all flash before your eyes.
    ->SensesReturn

*Smell
    You inhale sharply, smelling the flora of the city. The sharp fragrance reminds you of your past, and a newer, brighter future.
    ->SensesReturn

*Taste
    Your mouth starts to water as you taste once again. Breads, meats, fruits, familiar flavors from unfamiliar places.
    ->SensesReturn

*Hear
    You once again hear churchbells ring, birds chirping, a bustling city once again.
    ->SensesReturn
*{CHOICE_COUNT() ==0}
Feel
    You feel air escape you, the light from the cleansing grows brighter and you feel it's heat on you.
    **Feel
        You feel a burning sensation, little pins and needles assaulting into your skin.
        ***Feel
            You feel comfort, taking shelter in the warmth of the light, a familiar feeling. An embrace, her embrace.
            ****Breathe
                Then it goes silent.
                *****Continue
                    The journey was a long and treacherous.
                    ******Continue
                        The burden had been laid on your shoulders, and you succeeded.
                        ********Continue
                            At some point I thought I could bear loss, I could withstand saying goodbye.
                            *********Continue
                                But no pain was greater than letting you go. 
                                **********Continue
                                    As life returns to Radafell the more indebted we are to you.
                                    ***********Continue
                                        The more your sacrifice means.
                                        ************Continue
                                        The more I miss you...
                                            *************Continue
                                            My peony. ->END


->END

