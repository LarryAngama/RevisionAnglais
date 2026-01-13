using RevisionAnglais.Models;
using System.Collections.ObjectModel;

namespace RevisionAnglais.Services
{
    public class VerbService
    {
        private static List<VerbIrregulier> _allVerbs;

        public VerbService()
        {
            InitializeVerbs();
        }

        private void InitializeVerbs()
        {
            _allVerbs = new List<VerbIrregulier>
            {
                new VerbIrregulier(1, "be", "was/were", "être"),
                new VerbIrregulier(2, "beat", "beat", "battre"),
                new VerbIrregulier(3, "become", "became", "devenir"),
                new VerbIrregulier(4, "begin", "began", "commencer"),
                new VerbIrregulier(5, "bend", "bent", "plier"),
                new VerbIrregulier(6, "bite", "bit", "mordre"),
                new VerbIrregulier(7, "blow", "blew", "souffler"),
                new VerbIrregulier(8, "break", "broke", "casser"),
                new VerbIrregulier(9, "bring", "brought", "apporter"),
                new VerbIrregulier(10, "build", "built", "construire"),
                new VerbIrregulier(11, "burn", "burnt/burned", "brûler"),
                new VerbIrregulier(12, "buy", "bought", "acheter"),
                new VerbIrregulier(13, "catch", "caught", "attraper"),
                new VerbIrregulier(14, "choose", "chose", "choisir"),
                new VerbIrregulier(15, "come", "came", "venir"),
                new VerbIrregulier(16, "cost", "cost", "coûter"),
                new VerbIrregulier(17, "cut", "cut", "couper"),
                new VerbIrregulier(18, "deal", "dealt", "traiter"),
                new VerbIrregulier(19, "dig", "dug", "creuser"),
                new VerbIrregulier(20, "do", "did", "faire"),
                new VerbIrregulier(21, "draw", "drew", "dessiner/tirer"),
                new VerbIrregulier(22, "dream", "dreamt/dreamed", "rêver"),
                new VerbIrregulier(23, "drink", "drank", "boire"),
                new VerbIrregulier(24, "drive", "drove", "conduire"),
                new VerbIrregulier(25, "eat", "ate", "manger"),
                new VerbIrregulier(26, "fall", "fell", "tomber"),
                new VerbIrregulier(27, "feel", "felt", "sentir"),
                new VerbIrregulier(28, "fight", "fought", "combattre"),
                new VerbIrregulier(29, "find", "found", "trouver"),
                new VerbIrregulier(30, "fly", "flew", "voler"),
                new VerbIrregulier(31, "forget", "forgot", "oublier"),
                new VerbIrregulier(32, "forgive", "forgave", "pardonner"),
                new VerbIrregulier(33, "freeze", "froze", "geler"),
                new VerbIrregulier(34, "get", "got", "obtenir"),
                new VerbIrregulier(35, "give", "gave", "donner"),
                new VerbIrregulier(36, "go", "went", "aller"),
                new VerbIrregulier(37, "grow", "grew", "grandir/cultiver"),
                new VerbIrregulier(38, "hang", "hung", "pendre"),
                new VerbIrregulier(39, "have", "had", "avoir"),
                new VerbIrregulier(40, "hear", "heard", "entendre"),
                new VerbIrregulier(41, "hide", "hid", "se cacher"),
                new VerbIrregulier(42, "hit", "hit", "frapper"),
                new VerbIrregulier(43, "hold", "held", "tenir"),
                new VerbIrregulier(44, "hurt", "hurt", "blesser"),
                new VerbIrregulier(45, "keep", "kept", "garder"),
                new VerbIrregulier(46, "know", "knew", "connaître"),
                new VerbIrregulier(47, "lay", "laid", "poser"),
                new VerbIrregulier(48, "lead", "led", "mener"),
                new VerbIrregulier(49, "learn", "learnt/learned", "apprendre"),
                new VerbIrregulier(50, "leave", "left", "partir/laisser"),
                new VerbIrregulier(51, "lend", "lent", "prêter"),
                new VerbIrregulier(52, "let", "let", "laisser"),
                new VerbIrregulier(53, "lie", "lay", "s'allonger"),
                new VerbIrregulier(54, "lose", "lost", "perdre"),
                new VerbIrregulier(55, "make", "made", "faire/fabriquer"),
                new VerbIrregulier(56, "meet", "met", "rencontrer"),
                new VerbIrregulier(57, "pay", "paid", "payer"),
                new VerbIrregulier(58, "put", "put", "mettre"),
                new VerbIrregulier(59, "read", "read", "lire"),
                new VerbIrregulier(60, "ride", "rode", "chevaucher"),
                new VerbIrregulier(61, "ring", "rang", "sonner"),
                new VerbIrregulier(62, "rise", "rose", "se lever"),
                new VerbIrregulier(63, "run", "ran", "courir"),
                new VerbIrregulier(64, "say", "said", "dire"),
                new VerbIrregulier(65, "see", "saw", "voir"),
                new VerbIrregulier(66, "seek", "sought", "chercher"),
                new VerbIrregulier(67, "sell", "sold", "vendre"),
                new VerbIrregulier(68, "send", "sent", "envoyer"),
                new VerbIrregulier(69, "set", "set", "fixer"),
                new VerbIrregulier(70, "shake", "shook", "secouer"),
                new VerbIrregulier(71, "shine", "shone", "briller"),
                new VerbIrregulier(72, "shoot", "shot", "tirer"),
                new VerbIrregulier(73, "show", "showed", "montrer"),
                new VerbIrregulier(74, "shut", "shut", "fermer"),
                new VerbIrregulier(75, "sing", "sang", "chanter"),
                new VerbIrregulier(76, "sink", "sank", "couler"),
                new VerbIrregulier(77, "sit", "sat", "s'asseoir"),
                new VerbIrregulier(78, "sleep", "slept", "dormir"),
                new VerbIrregulier(79, "slide", "slid", "glisser"),
                new VerbIrregulier(80, "speak", "spoke", "parler"),
                new VerbIrregulier(81, "spend", "spent", "dépenser"),
                new VerbIrregulier(82, "split", "split", "diviser"),
                new VerbIrregulier(83, "spread", "spread", "étendre"),
                new VerbIrregulier(84, "stand", "stood", "se tenir debout"),
                new VerbIrregulier(85, "steal", "stole", "voler"),
                new VerbIrregulier(86, "stick", "stuck", "coller"),
                new VerbIrregulier(87, "sting", "stung", "piquer"),
                new VerbIrregulier(88, "stink", "stank", "sentir mauvais"),
                new VerbIrregulier(89, "strike", "struck", "frapper"),
                new VerbIrregulier(90, "string", "strung", "enfiler"),
                new VerbIrregulier(91, "strive", "strove", "s'efforcer"),
                new VerbIrregulier(92, "swear", "swore", "jurer"),
                new VerbIrregulier(93, "sweat", "sweat/sweated", "transpirer"),
                new VerbIrregulier(94, "sweep", "swept", "balayer"),
                new VerbIrregulier(95, "swell", "swelled", "enfler"),
                new VerbIrregulier(96, "swim", "swam", "nager"),
                new VerbIrregulier(97, "swing", "swung", "balancer"),
                new VerbIrregulier(98, "take", "took", "prendre"),
                new VerbIrregulier(99, "teach", "taught", "enseigner"),
                new VerbIrregulier(100, "tear", "tore", "déchirer"),
                new VerbIrregulier(101, "tell", "told", "dire"),
                new VerbIrregulier(102, "think", "thought", "penser"),
                new VerbIrregulier(103, "throw", "threw", "lancer"),
                new VerbIrregulier(104, "thrust", "thrust", "pousser"),
                new VerbIrregulier(105, "understand", "understood", "comprendre"),
                new VerbIrregulier(106, "upset", "upset", "déranger"),
                new VerbIrregulier(107, "wake", "woke", "se réveiller"),
                new VerbIrregulier(108, "wear", "wore", "porter"),
                new VerbIrregulier(109, "weave", "wove", "tisser"),
                new VerbIrregulier(110, "weep", "wept", "pleurer"),
                new VerbIrregulier(111, "win", "won", "gagner"),
                new VerbIrregulier(112, "wind", "wound", "enrouler"),
                new VerbIrregulier(113, "write", "wrote", "écrire"),
            };
        }

        public List<VerbIrregulier> GetAllVerbs()
        {
            return new List<VerbIrregulier>(_allVerbs);
        }

        public ObservableCollection<VerbIrregulier> GetVerbsAsObservable()
        {
            return new ObservableCollection<VerbIrregulier>(_allVerbs);
        }
    }
}
