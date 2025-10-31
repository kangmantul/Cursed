VAR clue_birthday = false

=== chat_with_caca ===
CACA: Hei, kamu masih ingat tanggal ulang tahunku?
*   [Ya, tentu saja!] 
    -> correct_answer
*   [Hmm... aku lupa] 
    -> forgot_answer

=== correct_answer ===
CACA: Wah hebat! Aku kira kamu udah lupa~
-> END

=== forgot_answer ===
{clue_birthday == false:
    CACA: Hmmm, kayaknya kamu butuh lihat-lihat file foto dulu deh.
- else:
    CACA: Oh! Kamu udah nemuin petunjuknya ya.
}
-> END
