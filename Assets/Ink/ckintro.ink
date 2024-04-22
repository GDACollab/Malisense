You open your eyes and see a familiar figure. Your throat tightens as she turns to you. Her warm smile softens the gloom of the decaying fountain. 
    
<b>??:</b> I don’t need my sight to recognize your presence, little sweet. It has been a long while since anyone has dared venture to this old place. It's rude to keep a lady waiting...even if that lady has all the time in the world.
    
    She giggles softly.

 * Who are you...again?
    <b>??:</b> You don't recall? Perhaps that is to be expected, considering your state. 
    She lets out a quiet sigh. 
    <b>??:</b> Well, I'm sure you'll remember soon enough. Let's just say you're quite special to me, and I'm quite special to you. ->whathappened
   
 * Hello?
    <b>??:</b> Oh, thank the heavens. I was so worried that, in all the tumult, you might have forgotten who I was. But I guess that's how peonies are: Just when you think they've wilted completely, they suddenly blossom brighter than ever before.
   You feel as if you know this person, but struggle to even recall her name. ->whathappened
   
   ==whathappened==
   *What happened?
   <b>??:</b> "Hmm...I guess there is no easy way to say this." The woman's face becomes pale. She fidgets with her necklace. "You...passed on. Oh, you can only imagine how distraught I was to find your lifeless body out here. Thankfully, I was able to collect myself and begin preparations to bring you back. I am the keeper of this crypt, after all. It took a fair bit of work, yes, but you know I'd do anything for my peony." ->whathappenedcont
    
   ==whathappenedcont==
    *How did you do such a thing?
   <b>CRYPT KEEPER:</b> A magician never reveals her secrets... but since you're special I'll make an exception. That lantern you hold contains your soul. When touching it, your spirit can once again animate your body as it did while you were still...living. I even carefully fitted your latern with some precious stones. If you're ever in danger, those gems will bring you back to me. I'm always here to take care of you, darling. When you feel the latern's glow, think of it as my embrace. ->lanterndesc
   
   ==lanterndesc==
    *You look down the lantern.
     Within it, a small candle burns soflty. The flicker of its flame is comforting. It reminds you of the broth you drank as a child when you were ill. The lantern itself is ornately sculpted. It's so polished that you can see yourself reflected in its silver casing. Someone clearly put a lot of effort into preparing it for this occassion. ->interrogation
     
    ==interrogation==
    *Where is everyone?
        CRYPT KEEPER: You don't remember? I'll tell you this much--I was right about that creature. Radefell is gone. The survivors are living in a nearby village. I've been helping them out, here and there. I bet they'll be quite happy to see you. But please, let's not discuss this any further. Besides, you're already in such a sorry state. I can't imagine that dwelling on bad news is making you feel any better. ->interrogation
    *Where *am I?
        <b>CRYPT KEEPER:</b> Apologies for the drab scenery. It's much easier to bring a soul back in a place such as this. Ressuciation is difficult in a city, where death is so omnnipresent. But I guess it's an apt setting, seeing as I've cleaned you up like the mother dove cleans her young in the fountain's basin. ->interrogation
    *Why revive me?
       <b>CRYPT KEEPER:</b> I'm not sure exactly, but I imagine it was quite painful. I found you not far from here, in a nearby cornfield. You were badly bruised, especially on your knees and elbows. It was a terrible scene, just awful. I was quite distraught but managed to pull myself together. "This won't do.", I told myself, "This won't do." So I picked you up--all of you, including the fingers you had lost--and put you back together. Don't worry, I didn't peek under your mask. I know you're awfully sensitive about that. ->interrogation
    *How did I die?
        She frowns. 
        <b>CRYPT KEEPER:</b> Why revive anyone else? You're the lantern of my life, peony. You should know that better than anyone else." ->interrogation

    *{CHOICE_COUNT() == 0} 
        <b>CRYPT KEEPER:</b> Now then, I'm afraid I must wish you farewell. But worry not, my darling, for we shall meet again very soon.
        She smiles at you, and--for the first time in a long time--you feel safe. You were safe at the chapel, sure, but any bird is safe in a cage. This is different. 
        The woman snaps her fingers and dissapears, leaving nothing behind but a faint aroma of lavender.

    -> END
