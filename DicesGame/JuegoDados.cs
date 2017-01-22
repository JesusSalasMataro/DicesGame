using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DicesGame
{
    public class JuegoDados
    {
        private readonly int MAX_TIRADAS = 3;
        private readonly int MIN_PUNTOS = 0;
        private readonly int MAX_PUNTOS = 50;

        #region "Métodos públicos"
        
        public void Run()
        {
            DadosDTO resultadoTirada = new DadosDTO();
            int puntos = 20;
            int numJugadas = 0;
            int numTiradas;
            int numDobles = 0;
            int puntosASumar, puntosARestar;
            bool finalizarJuego = false;

            Console.WriteLine("¡¡Bienvenido al juego de los dados!!");
            Console.WriteLine();
            Console.WriteLine("Tienes 3 tiradas de dados en cada jugada: ");
            Console.WriteLine("  - Si sacas un doble en la primera tirada sumas 20 puntos.");
            Console.WriteLine("  - Si sacas un doble en la segunda tirada sumas 10 puntos.");
            Console.WriteLine("  - Si sacas un doble en la tercera tirada sumas 5 puntos.");
            Console.WriteLine("  - Si no sacas un doble en las 3 tiradas restas 10 puntos.");
            Console.WriteLine();
            Console.WriteLine("Empiezas la partida con 20 puntos.");
            Console.WriteLine("Si te quedas con 0 o menos puntos pierdes.");
            Console.WriteLine("Si consigues 50 o más puntos ganas.");
            Console.WriteLine();
            Console.WriteLine("Pulsa cualquier tecla para empezar el juego...");
            Console.ReadKey();
            Console.Clear();

            while (!FinJuego(finalizarJuego, puntos))
            {
                numTiradas = 0;

                 while (!SacarDoble(resultadoTirada) && !MaximoTiradasAlcanzado(numTiradas))
                {                    
                    Console.WriteLine("Llevas " + numTiradas.ToString() + " " + 
                        TextoSingularPlural("tirada", numTiradas) + 
                        ", te " + 
                        TextoSingularPlural("queda", MAX_TIRADAS - numTiradas) + " " + 
                        (MAX_TIRADAS - numTiradas).ToString() + " " +
                        TextoSingularPlural("tirada", MAX_TIRADAS - numTiradas) + ".");
                    Console.WriteLine();

                    resultadoTirada = TirarDados();                    
                    Console.Clear();
                    numTiradas++;
                    MostrarTirada(resultadoTirada);
                }

                if (SacarDoble(resultadoTirada))
                {
                    puntosASumar = PuntosASumar(numTiradas);
                    puntos += puntosASumar;
                    numDobles++;

                    Console.WriteLine("¡Has sacado un doble! Sumas " + puntosASumar.ToString() + " puntos.");
                }
                else
                {
                    puntosARestar = PuntosARestar();
                    puntos -= puntosARestar;
                    
                    Console.WriteLine("No has sacado ningún doble, restas " + puntosARestar.ToString() + " puntos.");
                }

                resultadoTirada = BorrarDatosUltimaTirada();
                MostrarPuntuacionActual(puntos, numDobles);
                numJugadas++;

                finalizarJuego = !ContinuarJugando(puntos);
                Console.Clear();
            }

            MostrarResultadoFinJuego(puntos, numJugadas, numDobles);
            Console.ReadKey();
        }

        private string TextoSingularPlural(string texto, int numTiradas)
        {
            if (numTiradas == 1)
            {
                return texto;
            }
            else
            {
                if (texto.Equals("tirada"))
                {
                    return texto + "s";
                }
                else if (texto.Equals("queda"))
                {
                    return texto + "n";
                }
                else
                {
                    return texto;
                }
            }
        }

        #endregion

        #region "Método privados"        

        private bool FinJuego(bool finalizarJuego, int puntos)
        {
            return finalizarJuego || puntos <= MIN_PUNTOS || puntos >= MAX_PUNTOS;
        }

        private bool SacarDoble(DadosDTO resultadoTirada)
        {
            return resultadoTirada.Dado1 != 0 && resultadoTirada.Dado1 == resultadoTirada.Dado2;
        }

        private bool MaximoTiradasAlcanzado(int numTiradas)
        {
            return numTiradas == MAX_TIRADAS;
        }

        private DadosDTO TirarDados()
        {
            Console.WriteLine("Pulsa una tecla para tirar los dados...");
            Console.WriteLine();
            Console.ReadKey();
            Random random = new Random();
            DadosDTO tirada = new DadosDTO();
            tirada.Dado1 = random.Next(6) + 1;
            tirada.Dado2 = random.Next(6) + 1;

            return tirada;
        }

        private void MostrarTirada(DadosDTO resultadoTirada)
        {
            Console.WriteLine();
            Console.WriteLine("Primer dado:  " + resultadoTirada.Dado1);
            Console.WriteLine("Segundo dado: " + resultadoTirada.Dado2);
            Console.WriteLine();

            if (!SacarDoble(resultadoTirada))
            {
                Console.Write("No has sacado un doble, tendrás que intentarlo de nuevo.");
                Console.WriteLine();
            }
        }

        private int PuntosASumar(int numTiradas)
        {
            int puntos = 0;

            switch (numTiradas)
            {
                case 1:
                    puntos = 20;
                    break;
                case 2:
                    puntos = 10;
                    break;
                case 3:
                    puntos = 5;
                    break;
            }

            return puntos;
        }

        private int PuntosARestar()
        {
            return 10;
        }

        private DadosDTO BorrarDatosUltimaTirada()
        {
            return new DadosDTO();
        }

        private void MostrarPuntuacionActual(int puntos, int numDobles)
        {
            Console.WriteLine("Puntos: " + puntos.ToString() + ". Dobles: " + numDobles.ToString());
        }

        private bool ContinuarJugando(int puntos)
        {
            if (puntos > MIN_PUNTOS && puntos < MAX_PUNTOS)
            {
                Console.WriteLine();
                Console.WriteLine("¿Quieres seguir jugando? s/n ");
                
                return Console.ReadKey().KeyChar.ToString().ToLower().Equals("s");
            }
            else
            {
                return false;
            }
        }

        private void MostrarResultadoFinJuego(int puntos, int numJugadas, int numDobles)
        {
            Console.WriteLine("Juego finalizado.");
            Console.WriteLine();

            if (puntos <= MIN_PUNTOS)
            {
                Console.WriteLine("Has perdido :(");
                               
            }
            else if (puntos >= MAX_PUNTOS)
            {
                Console.WriteLine("¡¡Has ganado!! :D");
            }
            else
            {
                Console.WriteLine("Partida finalizada.");
            }

            Console.WriteLine();
            Console.WriteLine("Puntos: " + puntos.ToString() + ". Jugadas: " + numJugadas.ToString() +
                ". Dobles: " + numDobles.ToString());
        }

        #endregion
    }
}
