using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TicTacToe;

public class GameResult
{
    public string Player1 { get; set; }
    public string Player2 { get; set; }
    public string Board { get; set; }
}