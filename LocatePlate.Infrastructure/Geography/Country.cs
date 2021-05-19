using System.ComponentModel.DataAnnotations;

namespace LocatePlate.Infrastructure.Geography
{
    public enum Country
    {
        [Display(Name = "Select Country")]
        Unspecified,

        [Display(Name = "Andorra")]
        AD,

        [Display(Name = "United Arab Emirates")]
        AE,

        [Display(Name = "Afghanistan")]
        AF,

        [Display(Name = "Antigua and Barbuda")]
        AG,

        [Display(Name = "Anguilla")]
        AI,

        [Display(Name = "Albania")]
        AL,

        [Display(Name = "Armenia")]
        AM,

        [Display(Name = "Netherlands Antilles")]
        AN,

        [Display(Name = "Angola")]
        AO,

        [Display(Name = "Antarctica")]
        AQ,

        [Display(Name = "Argentina")]
        AR,

        [Display(Name = "American Samoa")]
        AS,

        [Display(Name = "Austria")]
        AT,

        [Display(Name = "Australia")]
        AU,

        [Display(Name = "Aruba")]
        AW,

        [Display(Name = "Åland Islands")]
        AX,

        [Display(Name = "Azerbaijan")]
        AZ,

        [Display(Name = "Bosnia and Herzegovina")]
        BA,

        [Display(Name = "Barbados")]
        BB,

        [Display(Name = "Bangladesh")]
        BD,

        [Display(Name = "Belgium")]
        BE,

        [Display(Name = "Burkina Faso")]
        BF,

        [Display(Name = "Bulgaria")]
        BG,

        [Display(Name = "Bahrain")]
        BH,

        [Display(Name = "Burundi")]
        BI,

        [Display(Name = "Benin")]
        BJ,

        [Display(Name = "Saint Barthélemy")]
        BL,

        [Display(Name = "Bermuda")]
        BM,

        [Display(Name = "Brunei Darussalam")]
        BN,

        [Display(Name = "Bolivia")]
        BO,

        [Display(Name = "Brazil")]
        BR,

        [Display(Name = "Bahamas")]
        BS,

        [Display(Name = "Bhutan")]
        BT,

        [Display(Name = "Bouvet Island")]
        BV,

        [Display(Name = "Botswana")]
        BW,

        [Display(Name = "Belarus")]
        BY,

        [Display(Name = "Belize")]
        BZ,

        [Display(Name = "Canada")]
        CA,

        [Display(Name = "Cocos (Keeling) Islands")]
        CC,

        [Display(Name = "Congo, the Democratic Republic of the")]
        CD,

        [Display(Name = "Central African Republic")]
        CF,

        [Display(Name = "Congo")]
        CG,

        [Display(Name = "Switzerland")]
        CH,

        [Display(Name = "Côte d'Ivoire")]
        CI,

        [Display(Name = "Cook Islands")]
        CK,

        [Display(Name = "Chile")]
        CL,

        [Display(Name = "Cameroon")]
        CM,

        [Display(Name = "China")]
        CN,

        [Display(Name = "Colombia")]
        CO,

        [Display(Name = "Costa Rica")]
        CR,

        [Display(Name = "Cuba")]
        CU,

        [Display(Name = "Cape Verde")]
        CV,

        [Display(Name = "Christmas Island")]
        CX,

        [Display(Name = "Cyprus")]
        CY,

        [Display(Name = "Czech Republic")]
        CZ,

        [Display(Name = "Germany")]
        DE,

        [Display(Name = "Djibouti")]
        DJ,

        [Display(Name = "Denmark")]
        DK,

        [Display(Name = "Dominica")]
        DM,

        [Display(Name = "Dominican Republic")]
        DO,

        [Display(Name = "Algeria")]
        DZ,

        [Display(Name = "Ecuador")]
        EC,

        [Display(Name = "Estonia")]
        EE,

        [Display(Name = "Egypt")]
        EG,

        [Display(Name = "Western Sahara")]
        EH,

        [Display(Name = "Eritrea")]
        ER,

        [Display(Name = "Spain")]
        ES,

        [Display(Name = "Ethiopia")]
        ET,

        [Display(Name = "Finland")]
        FI,

        [Display(Name = "Fiji")]
        FJ,

        [Display(Name = "Falkland Islands (Malvinas)")]
        FK,

        [Display(Name = "Micronesia, Federated States of")]
        FM,

        [Display(Name = "Faroe Islands")]
        FO,

        [Display(Name = "France")]
        FR,

        [Display(Name = "Gabon")]
        GA,

        [Display(Name = "United Kingdom")]
        GB,

        [Display(Name = "Grenada")]
        GD,

        [Display(Name = "Georgia")]
        GE,

        [Display(Name = "French Guiana")]
        GF,

        [Display(Name = "Guernsey")]
        GG,

        [Display(Name = "Ghana")]
        GH,

        [Display(Name = "Gibraltar")]
        GI,

        [Display(Name = "Greenland")]
        GL,

        [Display(Name = "Gambia")]
        GM,

        [Display(Name = "Guinea")]
        GN,

        [Display(Name = "Guadeloupe")]
        GP,

        [Display(Name = "Equatorial Guinea")]
        GQ,

        [Display(Name = "Greece")]
        GR,

        [Display(Name = "South Georgia and the South Sandwich Islands")]
        GS,

        [Display(Name = "Guatemala")]
        GT,

        [Display(Name = "Guam")]
        GU,

        [Display(Name = "Guinea-Bissau")]
        GW,

        [Display(Name = "Guyana")]
        GY,

        [Display(Name = "Hong Kong")]
        HK,

        [Display(Name = "Heard Island and McDonald Islands")]
        HM,

        [Display(Name = "Honduras")]
        HN,

        [Display(Name = "Croatia")]
        HR,

        [Display(Name = "Haiti")]
        HT,

        [Display(Name = "Hungary")]
        HU,

        [Display(Name = "Indonesia")]
        ID,

        [Display(Name = "Ireland")]
        IE,

        [Display(Name = "Israel")]
        IL,

        [Display(Name = "Isle of Man")]
        IM,

        [Display(Name = "India")]
        IN,

        [Display(Name = "British Indian Ocean Territory")]
        IO,

        [Display(Name = "Iraq")]
        IQ,

        [Display(Name = "Iran, Islamic Republic of")]
        IR,

        [Display(Name = "Iceland")]
        IS,

        [Display(Name = "Italy")]
        IT,

        [Display(Name = "Jersey")]
        JE,

        [Display(Name = "Jamaica")]
        JM,

        [Display(Name = "Jordan")]
        JO,

        [Display(Name = "Japan")]
        JP,

        [Display(Name = "Kenya")]
        KE,

        [Display(Name = "Kyrgyzstan")]
        KG,

        [Display(Name = "Cambodia")]
        KH,

        [Display(Name = "Kiribati")]
        KI,

        [Display(Name = "Comoros")]
        KM,

        [Display(Name = "Saint Kitts and Nevis")]
        KN,

        [Display(Name = "Korea, Democratic People's Republic of")]
        KP,

        [Display(Name = "Korea, Republic of")]
        KR,

        [Display(Name = "Kuwait")]
        KW,

        [Display(Name = "Cayman Islands")]
        KY,

        [Display(Name = "Kazakhstan")]
        KZ,

        [Display(Name = "Lao People's Democratic Republic")]
        LA,

        [Display(Name = "Lebanon")]
        LB,

        [Display(Name = "Saint Lucia")]
        LC,

        [Display(Name = "Liechtenstein")]
        LI,

        [Display(Name = "Sri Lanka")]
        LK,

        [Display(Name = "Liberia")]
        LR,

        [Display(Name = "Lesotho")]
        LS,

        [Display(Name = "Lithuania")]
        LT,

        [Display(Name = "Luxembourg")]
        LU,

        [Display(Name = "Latvia")]
        LV,

        [Display(Name = "Libyan Arab Jamahiriya")]
        LY,

        [Display(Name = "Morocco")]
        MA,

        [Display(Name = "Monaco")]
        MC,

        [Display(Name = "Moldova, Republic of")]
        MD,

        [Display(Name = "Montenegro")]
        ME,

        [Display(Name = "Saint Martin (French part)")]
        MF,

        [Display(Name = "Madagascar")]
        MG,

        [Display(Name = "Marshall Islands")]
        MH,

        [Display(Name = "Macedonia, the former Yugoslav Republic of")]
        MK,

        [Display(Name = "Mali")]
        ML,

        [Display(Name = "Myanmar")]
        MM,

        [Display(Name = "Mongolia")]
        MN,

        [Display(Name = "Macao")]
        MO,

        [Display(Name = "Northern Mariana Islands")]
        MP,

        [Display(Name = "Martinique")]
        MQ,

        [Display(Name = "Mauritania")]
        MR,

        [Display(Name = "Montserrat")]
        MS,

        [Display(Name = "Malta")]
        MT,

        [Display(Name = "Mauritius")]
        MU,

        [Display(Name = "Maldives")]
        MV,

        [Display(Name = "Malawi")]
        MW,

        [Display(Name = "Mexico")]
        MX,

        [Display(Name = "Malaysia")]
        MY,

        [Display(Name = "Mozambique")]
        MZ,

        [Display(Name = "Namibia")]
        NA,

        [Display(Name = "New Caledonia")]
        NC,

        [Display(Name = "Niger")]
        NE,

        [Display(Name = "Norfolk Island")]
        NF,

        [Display(Name = "Nigeria")]
        NG,

        [Display(Name = "Nicaragua")]
        NI,

        [Display(Name = "Netherlands")]
        NL,

        [Display(Name = "Norway")]
        NO,

        [Display(Name = "Nepal")]
        NP,

        [Display(Name = "Nauru")]
        NR,

        [Display(Name = "Niue")]
        NU,

        [Display(Name = "New Zealand")]
        NZ,

        [Display(Name = "Oman")]
        OM,

        [Display(Name = "Panama")]
        PA,

        [Display(Name = "Peru")]
        PE,

        [Display(Name = "French Polynesia")]
        PF,

        [Display(Name = "Papua New Guinea")]
        PG,

        [Display(Name = "Philippines")]
        PH,

        [Display(Name = "Pakistan")]
        PK,

        [Display(Name = "Poland")]
        PL,

        [Display(Name = "Saint Pierre and Miquelon")]
        PM,

        [Display(Name = "Pitcairn")]
        PN,

        [Display(Name = "Puerto Rico")]
        PR,

        [Display(Name = "Palestinian Territory, Occupied")]
        PS,

        [Display(Name = "Portugal")]
        PT,

        [Display(Name = "Palau")]
        PW,

        [Display(Name = "Paraguay")]
        PY,

        [Display(Name = "Qatar")]
        QA,

        [Display(Name = "Réunion")]
        RE,

        [Display(Name = "Romania")]
        RO,

        [Display(Name = "Serbia")]
        RS,

        [Display(Name = "Russian Federation")]
        RU,

        [Display(Name = "Rwanda")]
        RW,

        [Display(Name = "Saudi Arabia")]
        SA,

        [Display(Name = "Solomon Islands")]
        SB,

        [Display(Name = "Seychelles")]
        SC,

        [Display(Name = "Sudan")]
        SD,

        [Display(Name = "Sweden")]
        SE,

        [Display(Name = "Singapore")]
        SG,

        [Display(Name = "Saint Helena")]
        SH,

        [Display(Name = "Slovenia")]
        SI,

        [Display(Name = "Svalbard and Jan Mayen")]
        SJ,

        [Display(Name = "Slovakia")]
        SK,

        [Display(Name = "Sierra Leone")]
        SL,

        [Display(Name = "San Marino")]
        SM,

        [Display(Name = "Senegal")]
        SN,

        [Display(Name = "Somalia")]
        SO,

        [Display(Name = "Suriname")]
        SR,

        [Display(Name = "Sao Tome and Principe")]
        ST,

        [Display(Name = "El Salvador")]
        SV,

        [Display(Name = "Syrian Arab Republic")]
        SY,

        [Display(Name = "Swaziland")]
        SZ,

        [Display(Name = "Turks and Caicos Islands")]
        TC,

        [Display(Name = "Chad")]
        TD,

        [Display(Name = "French Southern Territories")]
        TF,

        [Display(Name = "Togo")]
        TG,

        [Display(Name = "Thailand")]
        TH,

        [Display(Name = "Tajikistan")]
        TJ,

        [Display(Name = "Tokelau")]
        TK,

        [Display(Name = "Timor-Leste")]
        TL,

        [Display(Name = "Turkmenistan")]
        TM,

        [Display(Name = "Tunisia")]
        TN,

        [Display(Name = "Tonga")]
        TO,

        [Display(Name = "Turkey")]
        TR,

        [Display(Name = "Trinidad and Tobago")]
        TT,

        [Display(Name = "Tuvalu")]
        TV,

        [Display(Name = "Taiwan, Province of China")]
        TW,

        [Display(Name = "Tanzania, United Republic of")]
        TZ,

        [Display(Name = "Ukraine")]
        UA,

        [Display(Name = "Uganda")]
        UG,

        [Display(Name = "United Kingdom")]
        UK,

        [Display(Name = "United States Minor Outlying Islands")]
        UM,

        [Display(Name = "United States")]
        US,

        [Display(Name = "Uruguay")]
        UY,

        [Display(Name = "Uzbekistan")]
        UZ,

        [Display(Name = "Holy See (Vatican City State)")]
        VA,

        [Display(Name = "Saint Vincent and the Grenadines")]
        VC,

        [Display(Name = "Venezuela")]
        VE,

        [Display(Name = "Virgin Islands, British")]
        VG,

        [Display(Name = "Virgin Islands, U.S.")]
        VI,

        [Display(Name = "Viet Nam")]
        VN,

        [Display(Name = "Vanuatu")]
        VU,

        [Display(Name = "Wallis and Futuna")]
        WF,

        [Display(Name = "Samoa")]
        WS,

        [Display(Name = "Yemen")]
        YE,

        [Display(Name = "Mayotte")]
        YT,

        [Display(Name = "South Africa")]
        ZA,

        [Display(Name = "Zambia")]
        ZM,

        [Display(Name = "Zimbabwe")]
        ZW
    }
}