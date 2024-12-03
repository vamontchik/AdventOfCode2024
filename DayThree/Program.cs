using System.Text;

var memory = File.ReadAllText(Path.GetFullPath(args[0]));

var index = 0;
var state = BufferState.EMPTY;
var runningTotal = 0;
var valueOne = 0;
var valueTwo = 0;
var areWeCountingNow = true;

while (index < memory.Length)
{
    runningTotal += EvaluateMemory(ref memory, ref state, ref index, ref valueOne, ref valueTwo, ref areWeCountingNow);
}

Console.WriteLine("The final result is {0}", runningTotal);

return;

static int EvaluateMemory(
    ref string memory,
    ref BufferState state,
    ref int index,
    ref int valueOne,
    ref int valueTwo,
    ref bool areWeCountingNow)
{
    var result = state switch
    {
        BufferState.EMPTY => EvaluateEMPTY(ref memory, ref state, ref index),
        BufferState.M => EvaluateM(ref memory, ref state, ref index),
        BufferState.MU => EvaluateMU(ref memory, ref state, ref index),
        BufferState.MUL => EvaluateMUL(ref memory, ref state, ref index),
        BufferState.MUL_LEFTP => EvaluateMUL_LEFTP(ref memory, ref state, ref index, ref valueOne),
        BufferState.MUL_LEFTP_VALUE => EvaluateMUL_LEFTP_VALUE(ref memory, ref state, ref index),
        BufferState.MUL_LEFTP_VALUE_COMMA => EvaluateMUL_LEFTP_VALUE_COMMA(ref memory, ref state, ref index, ref valueTwo),
        BufferState.MUL_LEFTP_VALUE_COMMA_VALUE => EvaluateMUL_LEFTP_VALUE_COMMA_VALUE(ref memory, ref state, ref index),
        BufferState.MUL_LEFTP_VALUE_COMMA_VALUE_RIGHTP => EvaluateMUL_LEFTP_VALUE_COMMA_VALUE_RIGHTP(ref state, ref valueOne, ref valueTwo),
        BufferState.D => EvaluateD(ref memory, ref state, ref index),
        BufferState.DO => EvaluateDO(ref memory, ref state, ref index),
        BufferState.DON => EvaluateDON(ref memory, ref state, ref index),
        BufferState.DON_APO => EvaluateDON_APO(ref memory, ref state, ref index),
        BufferState.DON_APO_T => EvaluateDON_APO_T(ref memory, ref state, ref index),
        BufferState.DON_APO_T_LEFTP => EvaluateDON_APO_T_LEFTP(ref memory, ref state, ref index),
        BufferState.DON_APO_T_LEFTP_RIGHTP => EvaluateDON_APO_T_LEFTP_RIGHTP(ref areWeCountingNow, ref state),
        BufferState.DO_LEFTP => EvaluateDO_LEFTP(ref memory, ref state, ref index),
        BufferState.DO_LEFTP_RIGHTP => EvaluateDO_LEFTP_RIGHTP(ref areWeCountingNow, ref state),
        _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
    };

    return areWeCountingNow ? result : 0;
}

static int EvaluateEMPTY(ref string memory, ref BufferState state, ref int index)
{
    var nextChar = memory[index];

    state = nextChar switch
    {
        'm' => BufferState.M,
        'd' => BufferState.D,
        _ => BufferState.EMPTY
    };

    ++index;

    return 0;
}

static int EvaluateM(ref string memory, ref BufferState state, ref int index) =>
    EvaluateNextChar(ref memory, ref state, ref index, 'u', BufferState.MU);

static int EvaluateMU(ref string memory, ref BufferState state, ref int index) =>
    EvaluateNextChar(ref memory, ref state, ref index, 'l', BufferState.MUL);

static int EvaluateMUL(ref string memory, ref BufferState state, ref int index) =>
    EvaluateNextChar(ref memory, ref state, ref index, '(', BufferState.MUL_LEFTP);

static int EvaluateMUL_LEFTP(ref string memory, ref BufferState state, ref int index, ref int valueOne) =>
    EvaluateVariableLengthNumber(ref memory, ref state, ref index, ref valueOne, BufferState.MUL_LEFTP_VALUE);

static int EvaluateMUL_LEFTP_VALUE(ref string memory, ref BufferState state, ref int index) =>
    EvaluateNextChar(ref memory, ref state, ref index, ',', BufferState.MUL_LEFTP_VALUE_COMMA);

static int EvaluateMUL_LEFTP_VALUE_COMMA(ref string memory, ref BufferState state, ref int index, ref int valueTwo) =>
    EvaluateVariableLengthNumber(ref memory, ref state, ref index, ref valueTwo, BufferState.MUL_LEFTP_VALUE_COMMA_VALUE);

static int EvaluateMUL_LEFTP_VALUE_COMMA_VALUE(ref string memory, ref BufferState state, ref int index) =>
    EvaluateNextChar(ref memory, ref state, ref index, ')', BufferState.MUL_LEFTP_VALUE_COMMA_VALUE_RIGHTP);

static int EvaluateMUL_LEFTP_VALUE_COMMA_VALUE_RIGHTP(ref BufferState state, ref int valueOne, ref int valueTwo)
{
    state = BufferState.EMPTY;
    return valueOne * valueTwo;
}

static int EvaluateD(ref string memory, ref BufferState state, ref int index) => 
    EvaluateNextChar(ref memory, ref state, ref index, 'o', BufferState.DO);

static int EvaluateDO(ref string memory, ref BufferState state, ref int index)
{
    var nextChar = memory[index];

    state = nextChar switch
    {
        'n' => BufferState.DON,
        '(' => BufferState.DO_LEFTP,
        _ => BufferState.EMPTY
    };

    ++index;

    return 0;
}

static int EvaluateDON(ref string memory, ref BufferState state, ref int index) => 
    EvaluateNextChar(ref memory, ref state, ref index, '\'', BufferState.DON_APO);

static int EvaluateDON_APO(ref string memory, ref BufferState state, ref int index) =>
    EvaluateNextChar(ref memory, ref state, ref index, 't', BufferState.DON_APO_T);

static int EvaluateDON_APO_T(ref string memory, ref BufferState state, ref int index) =>
    EvaluateNextChar(ref memory, ref state, ref index, '(', BufferState.DON_APO_T_LEFTP);

static int EvaluateDON_APO_T_LEFTP(ref string memory, ref BufferState state, ref int index) =>
    EvaluateNextChar(ref memory, ref state, ref index, ')', BufferState.DON_APO_T_LEFTP_RIGHTP);

static int EvaluateDON_APO_T_LEFTP_RIGHTP(ref bool countingToggle, ref BufferState state)
{
    countingToggle = false;
    state = BufferState.EMPTY;
    return 0;
}

static int EvaluateDO_LEFTP(ref string memory, ref BufferState state, ref int index) =>
    EvaluateNextChar(ref memory, ref state, ref index, ')', BufferState.DO_LEFTP_RIGHTP);

static int EvaluateDO_LEFTP_RIGHTP(ref bool countingToggle, ref BufferState state)
{
    countingToggle = true;
    state = BufferState.EMPTY;
    return 0;
}

static int EvaluateNextChar(
    ref string memory,
    ref BufferState state,
    ref int index,
    char charToCheckAgainst,
    BufferState nextStateOnSuccess)
{
    var nextChar = memory[index];

    state = nextChar == charToCheckAgainst ? nextStateOnSuccess : BufferState.EMPTY;

    ++index;

    return 0;
}

static int EvaluateVariableLengthNumber(
    ref string memory,
    ref BufferState state,
    ref int index,
    ref int toSaveTo,
    BufferState nextStateOnSuccess)
{
    // continue reading until we hit a non-number, and then attempt to parse
    var possibleNumberBuilder = new StringBuilder();
    while (index < memory.Length)
    {
        var nextChar = memory[index];
        var isNumber = char.IsNumber(nextChar);
        if (!isNumber)
            break;
        possibleNumberBuilder.Append(nextChar);
        ++index; // update index after if statement, so we don't go past the first non-numeric character
    }

    var valueStr = possibleNumberBuilder.ToString();
    if (string.IsNullOrEmpty(valueStr))
    {
        state = BufferState.EMPTY;
        return 0;
    }

    toSaveTo = int.Parse(valueStr);
    state = nextStateOnSuccess;

    return 0;
}

internal enum BufferState
{
    EMPTY,
    M,
    MU,
    MUL,
    MUL_LEFTP,
    MUL_LEFTP_VALUE,
    MUL_LEFTP_VALUE_COMMA,
    MUL_LEFTP_VALUE_COMMA_VALUE,
    MUL_LEFTP_VALUE_COMMA_VALUE_RIGHTP,
    D,
    DO,
    DON,
    DON_APO,
    DON_APO_T,
    DON_APO_T_LEFTP,
    DON_APO_T_LEFTP_RIGHTP,
    DO_LEFTP,
    DO_LEFTP_RIGHTP
}