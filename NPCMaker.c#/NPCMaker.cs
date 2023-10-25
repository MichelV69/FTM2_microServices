using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NPCMaker
{
  class NewFace
  {
    // --- static properties
    private static string[] TypeCodeList = {"Staff", "Patron", "Story Character"};
    private static string[] GenderCodeList = {"male", "female", "androgynous"};

    private Dictionary<string, int> RacialDistribution =
    new Dictionary<string, int>();

    private Dictionary<string, int> EyeColorTable =
    new Dictionary<string, int>();

    private Dictionary<string, int> HairColorTable =
    new Dictionary<string, int>();

    private Dictionary<string, int> HairStyleTable =
    new Dictionary<string, int>();

    // --- other class includes
    private DiceBagEngine diceBag = new DiceBagEngine();

    // --- variable properties
    private int TypeCode { get; set; }
    private string SexualOrientationText { get; set; }
    private int GenderCode { get; set; }
    private string PublicName { get; set; }
    public string TaskDesc { get; set; }
    private string Race { get; set; }
    private string HeightDesc { get; set; }
    private string BuildDesc { get; set; }
    private string HairColor { get; set; }
    private string HairStyle { get; set; }
    private string EyeColor { get; set; }
    private string QuirkEmotional { get; set; }
    private string QuirkPhysical { get; set; }
    private string NotableAttributePositive { get; set; }
    private string NotableAttributeNegative { get; set; }
    private string SchtickAbilityDescription { get; set; }

    // --- constructor
    public NewFace(string TypeCodeText, string NewTaskDesc)
    {
        this.PublicName = "(Character)";
        this.TypeCode = Array.IndexOf(TypeCodeList, TypeCodeText);
        this.TaskDesc = NewTaskDesc;

        RacialDistribution.Add("dragonborn", 1);
        RacialDistribution.Add("dwarf", 5);
        RacialDistribution.Add("elf", 4);
        RacialDistribution.Add("gnome", 3);
        RacialDistribution.Add("half-elf", 2);
        RacialDistribution.Add("halfling", 4);
        RacialDistribution.Add("half-orc", 1);
        RacialDistribution.Add("human", 7);
        RacialDistribution.Add("tiefling", 1);

        EyeColorTable.Add("brown", 32);
        EyeColorTable.Add("hazel", 16);
        EyeColorTable.Add("blue", 8);
        EyeColorTable.Add("green", 4);
        EyeColorTable.Add("grey", 2);
        EyeColorTable.Add("amber", 1);

        HairColorTable.Add("dark", 70);
        HairColorTable.Add("brown", 12);
        HairColorTable.Add("blonde", 6);
        HairColorTable.Add("white", 6);
        HairColorTable.Add("silver-grey", 6);
        HairColorTable.Add("red", 3);
        HairColorTable.Add("green", 1);
        HairColorTable.Add("blue", 1);

        HairStyleTable.Add("shaved clean", 1);
        HairStyleTable.Add("in a crew-cut", 1);
        HairStyleTable.Add("in a top-knot", 1);
        HairStyleTable.Add("in a short braid", 3);
        HairStyleTable.Add("in a few short braids", 3);
        HairStyleTable.Add("in a long braid", 1);
        HairStyleTable.Add("in a trio of long braids", 1);
        HairStyleTable.Add("in a bunch of beaded braids", 1);
        HairStyleTable.Add("in a short ponytail", 5);
        HairStyleTable.Add("in a long ponytail", 5);
        HairStyleTable.Add("short and loose", 5);
        HairStyleTable.Add("long and loose", 3);
        HairStyleTable.Add("in short curls", 5);
        HairStyleTable.Add("in long curls", 3);
        HairStyleTable.Add("in a cropped mohawk", 1);
        HairStyleTable.Add("in a short mohawk", 3);
        HairStyleTable.Add("in a long mohawk", 1);

    } // end method NPCMaker

    // --- other class methods
    public void RandomDetails()
    {
      // do some new do with the here do
      Race  = RandomWeightedRoller("Human*", RacialDistribution);

      HeightDesc  = getRandomHeightDesc();
      BuildDesc   = getRandomBuildDesc();

      GenderCode  = getRandomGenderCode();
      SexualOrientationText = RandomWeightedRoller("mind your own affairs, thanks", SexualOrientationTextTableReader());

      EyeColor   =  RandomWeightedRoller("Blue*", EyeColorTable);
      HairColor  =  RandomWeightedRoller("Chestnut*", HairColorTable);
      HairStyle  =  RandomWeightedRoller("Oily*", HairStyleTable);

      QuirkEmotional = RandomWeightedRoller("just fine, thanks", QuirkEmotionalTableReader());
      QuirkPhysical = RandomWeightedRoller("just fine, thanks", QuirkPhysicalTableReader());

      NotableAttributePositive = getNotableAttributePositive();
      NotableAttributeNegative = getNotableAttributeNegative();

      // SchtickAbilityDescription
    }

    public string toString()
    {
      // do some new do with the here do

      string word = "the";
      if (TypeCode > 0)
      {
        word = "a";
      }

      string patronInfoText = getTypeCodeText();
      if (TypeCode == 1)
      {
        int odds_here = Math.Max(diceBag.RollDice("1d8-1"),1);
        patronInfoText = $"Regular {getTypeCodeText()} (visiting: {odds_here} in 8)";
      }

      string visible = $"{patronInfoText} : {PublicName} is {word} {TaskDesc}. "
        + $"They are a {getGenderCodeText()} {Race}; {HeightDesc} and {BuildDesc}. "
        + $"They are {EyeColor}-eyed, with their {HairColor} hair kept {HairStyle}. ";

      string socText = "";
      if (SexualOrientationText != "-no-")
      {
        socText = $"They {SexualOrientationText}. ";
      }

      string qpText = "";
      if (QuirkPhysical != "-no-")
      {
        qpText = $"They have a {QuirkPhysical}. ";
      }
      string qeText = "";
      if (QuirkEmotional != "-no-")
      {
        qeText = $"They {QuirkEmotional}. ";
      }

      string quirkText = "";
      if ( (qpText + qeText).Length > 3)
      {
        quirkText = $"(Quirks:  {qpText} {qeText})";
      }

      string invisible = $"[GM Notes: {socText} {quirkText} {NotableAttributePositive} {NotableAttributeNegative}]";
      string paragraph = $"{visible}\n{invisible}";
      return paragraph;
    }

    public string getTypeCodeText()
    {
      return TypeCodeList[TypeCode];
    }

    private int getRandomGenderCode()
    {
      int roll_1d8 = diceBag.RollDice("1d8");
      int localGenderCode = -1;
      if (1 <= roll_1d8 && roll_1d8 <= 5)
      {
          localGenderCode = getGenderCodeByText("male");
      }
      if (6 <= roll_1d8 && roll_1d8 <= 8)
      {
          localGenderCode = getGenderCodeByText("female");
      }
      if (9 <= roll_1d8 && roll_1d8 <= 10)
      {
          localGenderCode = getGenderCodeByText("Androgenous");
      }
      return localGenderCode;
    } // end method getRandomGenderCode

    public int getGenderCodeByText(string GenderCodeText)
    {
      return Array.FindIndex(GenderCodeList, data => data.ToLower() == GenderCodeText.ToLower() );
    } // end method getGenderCodeByText

    private string getRandomHeightDesc()
    {
      string localHeightDesc = "average*";
      int roll_2d6 = diceBag.RollDice("2d6");

      if ( roll_2d6 == 2)
      {
          localHeightDesc = diceBag.SearchStringForRolls("very short (-[3d8+6]%)");
      }
      if ( roll_2d6 == 3)
      {
          localHeightDesc = diceBag.SearchStringForRolls("short (-[2d8+3]%)");
      }
      if (4 <= roll_2d6 && roll_2d6 <= 5)
      {
          localHeightDesc = diceBag.SearchStringForRolls("short-ish (-[1d8+1]%)");
      }

      if (6 <= roll_2d6 && roll_2d6 <= 8)
      {
          localHeightDesc = diceBag.SearchStringForRolls("average height ([2d4-4]%)");
      }

      if (9 <= roll_2d6 && roll_2d6 <= 10)
      {
          localHeightDesc = diceBag.SearchStringForRolls("tall-ish (+[1d8+1]%)");
      }
      if ( roll_2d6 == 11)
      {
          localHeightDesc = diceBag.SearchStringForRolls("tall (+[2d8+3]%)");
      }
      if ( roll_2d6 == 12)
      {
          localHeightDesc = diceBag.SearchStringForRolls("very tall (+[3d8+6]%)");
      }

      return localHeightDesc;
    } // end method getRandomHeightDesc

    public string getGenderCodeText()
    {
      return GenderCodeList[GenderCode];
    } // end method getGenderCodeText

    private string getRandomBuildDesc()
    {
      string localBuildDesc = "average*";
      int roll_2d6 = diceBag.RollDice("2d6");

      if ( roll_2d6 == 2)
      {
          localBuildDesc = diceBag.SearchStringForRolls("gaunt (-[3d8+6]%)");
      }
      if ( roll_2d6 == 3)
      {
          localBuildDesc = diceBag.SearchStringForRolls("lean (-[2d8+3]%)");
      }
      if (4 <= roll_2d6 && roll_2d6 <= 5)
      {
          localBuildDesc = diceBag.SearchStringForRolls("slightly angular (-[1d8+1]%)");
      }

      if (6 <= roll_2d6 && roll_2d6 <= 8)
      {
          localBuildDesc = diceBag.SearchStringForRolls("medium build ([2d4-4]%)");
      }

      if (9 <= roll_2d6 && roll_2d6 <= 10)
      {
          localBuildDesc = diceBag.SearchStringForRolls("slightly husky (+[1d8+1]%)");
      }
      if ( roll_2d6 == 11)
      {
          localBuildDesc = diceBag.SearchStringForRolls("stout (+[2d8+3]%)");
      }
      if ( roll_2d6 == 12)
      {
          localBuildDesc = diceBag.SearchStringForRolls("portly (+[3d8+6]%)");
      }

      return localBuildDesc;
    } // end method getRandomBuildDesc

    private string RandomWeightedRoller(string DefaultResult, Dictionary<string, int> WeightedTable)
    {

      string ResultText = DefaultResult;

      // add up total_weight
      int total_weight = 0;
      foreach (var TableData in WeightedTable)
      {
        total_weight += TableData.Value;
      }

      // roll from 1 to total_weight
      int RollToCompare = diceBag.RawRoll1To(total_weight);

      // loop the race list
      int RangeLow  = 0;
      int RangeHigh = 0;
      foreach (var TableData in WeightedTable)
      {
        // move high to low, and add the weight of the next race to high
        RangeLow = RangeHigh + 1;
        RangeHigh += TableData.Value;

        // if we're between high and low, that's our race
        if (RangeLow <= RollToCompare && RollToCompare <= RangeHigh)
        {
          ResultText = TableData.Key;
        }
      } // end foreach

      return ResultText;

    } // end method RandomWeightedRoller

    private Dictionary<string, int> LoadDictionaryTableFromFile(string FileToLoad,  string DetailFormatString, string[] DetailArray1,
      string[] DetailArray2)
    {
      Dictionary<string, int> dictionaryTableFromFile = new Dictionary<string, int>();
      string fileToLoad = Path.Combine(Environment.CurrentDirectory, $"table_data/{FileToLoad}");
      List<string> fileData = File.ReadAllLines(fileToLoad).ToList();

      fileData.ForEach(line =>
      {
        int rollWeight = Int32.Parse(line.Split('|').First());
        string textValue = line.Split('|').Last().Trim();

        if (textValue != "-no-")
        {
          string detailString1 = "";
          string detailString2 = "";

          if (DetailArray1.Any())
          {
            int detailArrayIndex = diceBag.RawRoll1To(DetailArray1.Count() - 1);
            detailString1 = DetailArray1[detailArrayIndex];
          }
          if (DetailArray2.Any())
          {
            int detailArrayIndex = diceBag.RawRoll1To(DetailArray2.Count() - 1);
            detailString2 = DetailArray2[detailArrayIndex];
          }

          textValue = string.Format(DetailFormatString, textValue, detailString1,
            detailString2);
        }

        dictionaryTableFromFile.Add(textValue, rollWeight);
      });

      return dictionaryTableFromFile;
    } // end method LoadDictionaryTableFromFile

    private Dictionary<string, int> QuirkPhysicalTableReader()
    {
      string fileToLoad = "NPCMaker.QuirkPhysicalTable.data";
      string detailFormatString = "{0} on their {1} {2}";
      string[] sideName = {"left", "right"};
      string[] locationNames = {"hand", "forearm", "upper arm", "shoulder", "cheek", "leg", "thigh", "collar-bone", "brow" };

      return LoadDictionaryTableFromFile(fileToLoad, detailFormatString, sideName, locationNames);
    } // end method QuirkPhysicalTableReader

    private Dictionary<string, int> QuirkEmotionalTableReader()
    {
      string fileToLoad = "NPCMaker.QuirkEmotionalTable.data";
      string detailFormatString = "{1} {0}";
      string[] prepPhrase = {"can sometimes be", "are often", "tend to be"};
      string[] unused = { };

      return LoadDictionaryTableFromFile(fileToLoad, detailFormatString, prepPhrase, unused);
    } // end method QuirkEmotionalTableReader

    private Dictionary<string, int> SexualOrientationTextTableReader()
    {
      string fileToLoad = "NPCMaker.SexualOrientationTextList.data";
      string detailFormatString = "{1} {0}";
      string[] unused = { };
      string[] prepPhrase =
      {
        "are quietly", "are flirtatiously", "consider themselves", "consider themselves", "consider themselves",
        "consider themselves"
      };

      return LoadDictionaryTableFromFile(fileToLoad, detailFormatString, prepPhrase, unused);
    } // end method QuirkEmotionalTableReader

    private string getNotableAttributePositive()
    {
      string RandomNotableAttributePositiveText = "";
      string[] unusedArray = {};
      Dictionary<string, int> NotableAttributeBonus = LoadDictionaryTableFromFile(
        "NPCMaker.NotableAttributeBonus.data",
        "{0}", unusedArray, unusedArray);
      Dictionary<string, int> NotableAttributeStat = LoadDictionaryTableFromFile(
        "NPCMaker.NotableAttributeStat.data",
        "{0}", unusedArray, unusedArray);

      int numberOfAttributes = int.Parse(RandomWeightedRoller("0", NotableAttributeBonus));
      for (int loopCount = 0; loopCount < numberOfAttributes; loopCount++)
      {
        string AtributeNameText = RandomWeightedRoller("0", NotableAttributeStat);
        int AtributeNameBonus = int.Parse(RandomWeightedRoller("0", NotableAttributeBonus));
        if (AtributeNameText != "-no-")
        {
          RandomNotableAttributePositiveText += $"[{AtributeNameText}: +{AtributeNameBonus}]";
        }
      } // end for-loopCount

      if (3 < RandomNotableAttributePositiveText.Length)
      {
        RandomNotableAttributePositiveText = $"Particularly Good At: {RandomNotableAttributePositiveText}";
      }

      return RandomNotableAttributePositiveText;
    } // end method RandomNotableAttributePositive

    private string getNotableAttributeNegative()
    {
      string RandomNotableAttributeNegativeText = "";
      string[] unusedArray = {};
      Dictionary<string, int> NotableAttributeBonus = LoadDictionaryTableFromFile(
        "NPCMaker.NotableAttributeBonus.data",
        "{0}", unusedArray, unusedArray);
      Dictionary<string, int> NotableAttributeStat = LoadDictionaryTableFromFile(
        "NPCMaker.NotableAttributeStat.data",
        "{0}", unusedArray, unusedArray);

      // we would rather fewer "bad" NPCs, so we're going to dope the table
      if (NotableAttributeStat.ContainsKey("-no-"))
      {
        NotableAttributeStat["-no-"] = NotableAttributeStat["-no-"]* 2;
      }

      // we further don't want them "bad" at their _prinary job_
      NotableAttributeStat.Remove("(Var) Trade Skill");

      int numberOfAttributes = int.Parse(RandomWeightedRoller("0", NotableAttributeBonus));
      for (int loopCount = 0; loopCount < numberOfAttributes; loopCount++)
      {
        string AtributeNameText = RandomWeightedRoller("0", NotableAttributeStat);
        int AtributeNameBonus = int.Parse(RandomWeightedRoller("0", NotableAttributeBonus));
        if (AtributeNameText != "-no-")
        {
          RandomNotableAttributeNegativeText += $"[{AtributeNameText}: -{AtributeNameBonus}]";
        }
      } // end for-loopCount

      if (3 < RandomNotableAttributeNegativeText.Length)
      {
        RandomNotableAttributeNegativeText = $"Particularly Bad At: {RandomNotableAttributeNegativeText}";
      }

      return RandomNotableAttributeNegativeText;
    } // end method RandomNotableAttributeNegative

    public string getRandomTaskDesc()
    {
      string[] unusedArray = {};
      Dictionary<string, int> RandomTaskDescList = LoadDictionaryTableFromFile(
        "NPCMaker.RandomTaskDescList.data",
        "{0}", unusedArray, unusedArray);

      return diceBag.SearchStringForRolls(RandomWeightedRoller("Barony's Most Interesting Person", RandomTaskDescList));
    }
  } // end class NPCMaker
}  // end namespace PBHouse_CLI
