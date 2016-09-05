using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using MedinetWeb.Models;

using DotNet.Highcharts;
using DotNet.Highcharts.Attributes;
using DotNet.Highcharts.Options;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Enums;
using Point = DotNet.Highcharts.Options.Point;
using System.Drawing;

namespace MedinetWeb.Controllers
{
    [Authorize]
    [SessionExpireFilter]
    public class HomeController : Controller
    {
        private MedinetWebEntities db = new MedinetWebEntities();

        [Authorize(Roles = "Administrador, Invitado, Ventas, Mercadeo, Visitador Web")]
        [SessionExpireFilter]
        public ActionResult Index()
        {
            

            var cicloAct = db.SP_MW_Ciclos_Act().First();
            ViewBag.Ciclo = cicloAct.NU_Ano.ToString() + cicloAct.NU_Ciclo.ToString();
            ViewBag.FEIni = cicloAct.FE_CicloIni.Day + "/" + cicloAct.FE_CicloIni.Month + "/" + cicloAct.FE_CicloIni.Year;
            ViewBag.FEFin = cicloAct.FE_CicloFin.Day + "/" + cicloAct.FE_CicloFin.Month + "/" + cicloAct.FE_CicloFin.Year;

            DateTime inicio = DateTime.Now;
            DateTime fin = new DateTime(cicloAct.FE_CicloFin.Year, cicloAct.FE_CicloFin.Month, cicloAct.FE_CicloFin.Day);
            TimeSpan restantes = (fin - inicio);
            if (restantes.Days < 0)
                ViewBag.Restantes = 0;
            else
                ViewBag.Restantes = restantes.Days + 1;
            ChartsModel model = new ChartsModel();
            model.Charts = new List<Highcharts>();

            string[] regiones = db.SP_MW_Dashboard_CoberturaPorReg().ToList()
                .OrderBy(e => e.TX_Region)
                .Select(c => c.TX_Region).ToArray();

            object[] valPuntos = db.SP_MW_Dashboard_CoberturaPorReg()
                .OrderBy(e => e.TX_Region)
                .Select(c => c.PCJFVRR.ToString()).ToArray();

            object[] valCoberturaMedica = db.SP_MW_Dashboard_CoberturaPorReg()
                .OrderBy(e => e.TX_Region)
                .Select(c => new object[]
                    { 
                        c.PCJFVRR
                    }).ToArray();

            object[] valCoberturaHospital = db.SP_MW_Dashboard_CoberturaPorReg()
                .OrderBy(e => e.TX_Region)
                .Select(c => new object[]
                    { 
                        c.VisitadosHospital
                    }).ToArray();

            object[] valCoberturaFarmacia = db.SP_MW_Dashboard_CoberturaPorReg()
                .OrderBy(e => e.TX_Region)
                .Select(c => new object[]
                    { 
                        c.VisitadosFarmacia
                    }).ToArray();


            List<Point> misPuntos = new List<Point>();
            int i = 0;
            foreach (object v in valPuntos)
            {
                Point punto = new Point
                {
                    Y = Convert.ToDouble(v),

                    Color = Color.FromName("colors[" + i + "]")
                };
                misPuntos.Add(punto);
                i++;
            }

            Highcharts chart1 = new DotNet.Highcharts.Highcharts("chart1")
                .SetCredits(new Credits { Enabled = false })
                .InitChart(new Chart { DefaultSeriesType = ChartTypes.Column, Height = 250 })
                .SetTitle(new Title { Text = "Coberturas" })
                .SetYAxis(new YAxis
                {
                    Title = new YAxisTitle
                    {
                        Text = "(%) Porcentajes"
                    }
                })
                .SetXAxis(new XAxis
                {
                    Title = new XAxisTitle
                    {
                        Text = "Regiones"
                    },
                    Categories = regiones
                })
                .SetSeries(new[]{
                    new Series
                    {
                        Name = "Cobertura Médica (Mix)",
                        Data = new Data(valCoberturaMedica)
                    },

                    new Series
                    {
                        Name = "Cobertura Farmacias",
                        Data = new Data(valCoberturaFarmacia)
                    },

                    new Series
                    {
                        Name = "Cobertura Hospitales",
                        Data = new Data(valCoberturaHospital)
                    },
                });


            var valAltasBajas = db.SP_MW_Dashboard_AltasBajas().First();

            Highcharts chart3 = new Highcharts("chart3")
                .SetCredits(new Credits { Enabled = false })
                .InitChart(new Chart { Height = 200 })
                .SetTitle(new Title { Text = "Altas y Bajas" })
                .SetTooltip(new Tooltip { Formatter = "function() { return '<b>'+ this.point.name +'</b>: '+ this.percentage +' %'; }" })
                .SetTooltip(new Tooltip { Formatter = "TooltipFormatter" })
                .SetXAxis(new XAxis { Categories = new[] { "Altas", "Bajas" } })
                .AddJavascripFunction("TooltipFormatter",
                                      @"var s;
                    if (this.point.name) { // the pie chart
                       s = ''+
                          this.point.name +': '+ this.y +'%';
                    } else {
                       s = ''+
                          'Médicos: '+ this.y;
                    }
                    return s;")
                .SetLabels(new Labels
                {
                    Items = new[]
                            {
                                new LabelsItems
                                    {
                                        Html = "% Altas y Bajas",
                                        Style = "left: '165px', top: '8px', color: 'black'"
                                    }
                            }
                })
                .SetYAxis(new YAxis
                {
                    Title = new YAxisTitle
                    {
                        Text = "Cantidad de Médicos"
                    }
                })
                .SetPlotOptions(new PlotOptions
                {
                    Pie = new PlotOptionsPie
                    {
                        Center = new[] { new PercentageOrPixel(170), new PercentageOrPixel(40) },
                        Size = new PercentageOrPixel(60),
                        ShowInLegend = false,
                        DataLabels = new PlotOptionsPieDataLabels { Enabled = false }
                    },
                    Column = new PlotOptionsColumn
                    {
                        ShowInLegend = false,
                        DataLabels = new PlotOptionsColumnDataLabels { Enabled = true }
                    }
                })
                .SetSeries(new[]
                    {
                        new Series
                            {
                                Type = ChartTypes.Column,
                                Data = new Data(new[] { 
                                    new Point
                                            {
                                                //Name = "Altas",
                                                DataLabels = false,
                                                Y = valAltasBajas.NU_Altas,
                                                Color = Color.FromName("Highcharts.getOptions().colors[0]")
                                            },
                                        new Point
                                            {
                                                //Name = "Bajas",
                                                DataLabels = false,
                                                Y = valAltasBajas.NU_Bajas,
                                                Color = Color.FromName("Highcharts.getOptions().colors[1]")
                                            }
                                
                                })
                            },
                        
                        new Series
                            {
                                Type = ChartTypes.Pie,
                                Name = "Total",
                                Data = new Data(new[]
                                    {
                                        new Point
                                            {
                                                Name = "Altas",
                                                Y = Convert.ToDouble(valAltasBajas.NU_PRCAltas),
                                                Color = Color.FromName("Highcharts.getOptions().colors[0]")
                                            },
                                        new Point
                                            {
                                                Name = "Bajas",
                                                Y = Convert.ToDouble(valAltasBajas.NU_PRCBajas),
                                                Color = Color.FromName("Highcharts.getOptions().colors[1]")
                                            }
                                    }
                               )
                            }
                    });


            var valAVGFichas = db.SP_MW_Dashboard_PromedioFichas().First();

            //IMPORTANTE Para que el Angular Gauge funcione debe estar en el _Layout el script "highcharts-more.js"
            Highcharts chart4 = new Highcharts("chart4")
                .SetCredits(new Credits { Enabled = false })
                .InitChart(new Chart
                {
                    Type = ChartTypes.Gauge,
                    PlotBackgroundColor = null,
                    PlotBackgroundImage = null,
                    PlotBorderWidth = 0,
                    PlotShadow = false,
                    Height = 250
                })
                .SetTitle(new Title { Text = "% de Fichas Llenas" })
                .SetPane(new Pane
                {
                    StartAngle = -150,
                    EndAngle = 120,
                    Background = new[]
                            {
                                new BackgroundObject
                                    {
                                        BackgroundColor = new BackColorOrGradient(new Gradient
                                            {
                                                LinearGradient = new[] { 0, 0, 0, 1 },
                                                Stops = new object[,] { { 0, "#FFF" }, { 1, "#333" } }
                                            }),
                                        BorderWidth = new PercentageOrPixel(0),
                                        OuterRadius = new PercentageOrPixel(109, true)
                                    },
                                new BackgroundObject
                                    {
                                        BackgroundColor = new BackColorOrGradient(new Gradient
                                            {
                                                LinearGradient = new[] { 0, 0, 0, 1 },
                                                Stops = new object[,] { { 0, "#333" }, { 1, "#FFF" } }
                                            }),
                                        BorderWidth = new PercentageOrPixel(1),
                                        OuterRadius = new PercentageOrPixel(107, true)
                                    },
                                new BackgroundObject(),
                                new BackgroundObject
                                    {
                                        BackgroundColor = new BackColorOrGradient(ColorTranslator.FromHtml("#DDD")),
                                        BorderWidth = new PercentageOrPixel(0),
                                        OuterRadius = new PercentageOrPixel(105, true),
                                        InnerRadius = new PercentageOrPixel(103, true)
                                    }
                            }
                })
                .SetYAxis(new YAxis
                {
                    Min = 0,
                    Max = 100,

                    //MinorTickInterval = "auto",
                    MinorTickWidth = 1,
                    MinorTickLength = 10,
                    MinorTickPosition = TickPositions.Inside,
                    MinorTickColor = ColorTranslator.FromHtml("#666"),
                    TickPixelInterval = 30,
                    TickWidth = 2,
                    TickPosition = TickPositions.Inside,
                    TickLength = 10,
                    TickColor = ColorTranslator.FromHtml("#666"),
                    Labels = new YAxisLabels
                    {
                        Step = 2,
                        //Rotation = "auto"
                    },
                    Title = new YAxisTitle { Text = "% Fichas" },
                    PlotBands = new[]
                            {
                                new YAxisPlotBands { From = 0, To = 50, Color = ColorTranslator.FromHtml("#DF5353") },
                                new YAxisPlotBands { From = 50, To = 90, Color = ColorTranslator.FromHtml("#DDDF0D") },
                                new YAxisPlotBands { From = 90, To = 100, Color = ColorTranslator.FromHtml("#55BF3B") }
                            }
                })
                .SetSeries(new Series
                {
                    Name = "Valor",
                    Data = new Data(new object[] { valAVGFichas.Value })
                });

            model.Charts.Add(chart1);
            //model.Charts.Add(chart2);
            model.Charts.Add(chart3);
            model.Charts.Add(chart4);

            return View(model);
        }

        [Authorize(Roles = "Administrador, Invitado, Ventas, Mercadeo")]
        [SessionExpireFilter]
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        [Authorize(Roles = "Administrador, Invitado, Ventas, Mercadeo")]
        [SessionExpireFilter]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }

    public class ChartsModel
    {
        public List<Highcharts> Charts { get; set; }
    }

    public class TopVisitadores
    {
        public static MedinetWebEntities db = new MedinetWebEntities();

        public TopVisitadores(int? orden, string visitador, decimal mix, string region, string linea)
        {
            Orden = orden;
            Visitador = visitador;
            Mix = mix;
            Region = region;
            Linea = linea;
        }

        public int? Orden { get; set; }
        public TopVisitadores()
        {
            // TODO: Complete member initialization
        }
        public string Visitador { get; set; }
        public decimal? Mix { get; set; }
        public string Region { get; set; }
        public string Linea { get; set; }

        public static List<TopVisitadores> getTopVisitadores()
        {
            List<TopVisitadores> miTopVisitadores = new List<TopVisitadores>();
            miTopVisitadores = db.SP_MW_Dashboard_TopVisitadores()
                                    .Select(c => new TopVisitadores
                                    {
                                        Orden = c.Orden,
                                        Visitador = c.Visitador,
                                        Mix = c.Mix,
                                        Region = c.TX_Region,
                                        Linea = c.TX_Linea
                                    }).ToList<TopVisitadores>();

            return miTopVisitadores;
        }
    }
}
