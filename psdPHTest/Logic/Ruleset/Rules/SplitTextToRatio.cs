using System;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static psdPH.Utils.SplitTextToRatio;

namespace psdPHTest.Logic.Ruleset.Rules
{
    [TestClass]
    public partial class SplitTextToRatio
    {
        [DataTestMethod]
        [DataRow("suroundings of the wild beasts", (double) 4/1)]
        [DataRow("zenless zone zero", (double)1 /1)]
        [DataRow("подкаст на тему \"еда и спорт\"", (double)5 /2)]
        [DataRow("left 4 dead", (double) 1 /2)]
        [DataRow("the legend of zelda: breath of the wild", (double)16 / 9)]
        [DataRow("science and technology weekly digest", (double)3 / 2)]
        [DataRow("путешествие по горам Кавказа", (double)4 / 3)]
        [DataRow("cyberpunk 2077: phantom liberty", (double)21 / 9)]
        [DataRow("morning coffee with jazz music", (double)1 / 1)]
        [DataRow("искусственный интеллект в современном мире", (double)5 / 4)]
        [DataRow("the witcher 3: wild hunt", (double)16 / 10)]
        [DataRow("documentary: life in the ocean", (double)2 / 1)]
        [DataRow("рецепты домашней выпечки", (double)3 / 4)]
        [DataRow("starfield: into the unknown", (double)32 / 9)]
        [DataRow("the last of us part II", (double)18 / 9)]
        [DataRow("космические исследования 2024", (double)5 / 3)]
        [DataRow("elden ring: shadow of the erdtree", (double)16 / 9)]
        [DataRow("история древнего Рима", (double)4 / 5)]
        [DataRow("deep learning for beginners", (double)3 / 2)]
        [DataRow("red dead redemption 2", (double)21 / 9)]
        [DataRow("фитнес-трекер и здоровье", (double)1 / 1)]
        [DataRow("the matrix: resurrections", (double)2.35 / 1)]
        [DataRow("путеводитель по Японии", (double)16 / 10)]
        [DataRow("quantum computing explained", (double)4 / 3)]
        [DataRow("stray: lost in the city", (double)5 / 4)]
        [DataRow("как научиться программировать", (double)3 / 4)]
        [DataRow("interstellar: beyond the stars", (double)2.39 / 1)]
        [DataRow("современное искусство", (double)1 / 2)]
        [DataRow("god of war: ragnarök", (double)19 / 9)]
        [DataRow("биткоин и криптовалюты", (double)16 / 9)]
        [DataRow("the sims 4: домашний уют", (double)16 / 10)]
        [DataRow("астрономия для начинающих", (double)4 / 3)]
        [DataRow("death stranding: director's cut", (double)32 / 9)]
        [DataRow("лучшие книги 2024 года", (double)1 / 1)]
        public void testSplit(string str,double ratio)
        {
            var splitted = Splitter.Split(str,ratio*2);
            Console.WriteLine(splitted);
        }
    }
}
