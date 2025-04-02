using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TicTacToe;


public class Participant
{
    public string Name { get; set; }
    public string Country { get; set; }
}
