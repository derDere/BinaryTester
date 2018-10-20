#include <fstream>


using namespace std;


const char NEXT_COMMAND = 29; // GS
const char NEXT_PART = 30;    // RS
const char NEXT_PARAM = 31;   // US
const char APP_END = 28;      // FS

const char CONNECTION = 92;   // \ 
const char DATA_POINT = 46;   // .

const char LEAVER = 124; // |
const char NEGATE = 33; // !
const char AND = 38; // &
const char LAMP = 76; // L
const char XOR = 88; // X
const char REPEATER = 62; // >
const char TICKER = 58; // :
const char FLIP_FLOP = 70; // F
const char T_FLIP_FLOP = 84; // T
const char BUTTON = 66; // B
const char ANCHOR = 35; // #
const char PORTAL = 80; // P
const char EXTENDER = 45; // -
const char COUNTER = 67; // C


struct sConnection
{
	unsigned short from; //Source DataPoint
	unsigned short to; //Target Datapoint
	sConnection* next; //Next Connection Object
};


class cComponent{
public:
	bool** input; //List Of Input Datapoints
	int inputCount = 0; //Input Datapoint List Length
	bool** output; //List Of Output Datapoints
	int outputCount = 0; //Output Datapoint List Length
	cComponent* next; //Next Component Object
	char type = '\0'; //Component Type

	cComponent(ifstream* fs, bool* dataPoints);

	virtual void update();
};


class LeaverComp : public cComponent{
public:
	bool state = false;
	char type = LEAVER;
	void update();

	LeaverComp(ifstream* fs, bool* dataPoints);
};


class NegateComp : public cComponent{
public:
	char type = NEGATE;
	void update();

	NegateComp(ifstream* fs, bool* dataPoints);
};


class AndComp : public cComponent{
public:
	char type = AND;
	void update();

	AndComp(ifstream* fs, bool* dataPoints);
};


class LampComp : public cComponent{
public:
	bool state = false;
	char type = LAMP;
	void update();

	LampComp(ifstream* fs, bool* dataPoints);
};


class XorComp : public cComponent{
public:
	char type = XOR;
	void update();

	XorComp(ifstream* fs, bool* dataPoints);
};


class RepeaterComp : public cComponent{
public:
	char type = REPEATER;
	void update();

	RepeaterComp(ifstream* fs, bool* dataPoints);
};


class TickerComp : public cComponent{
public:
	char type = TICKER;
	void update();

	TickerComp(ifstream* fs, bool* dataPoints);
};


class FlipFlopComp : public cComponent{
public:
	char type = FLIP_FLOP;
	void update();

	FlipFlopComp(ifstream* fs, bool* dataPoints);
};


class TFlipFlopComp : public cComponent{
public:
	char type = T_FLIP_FLOP;
	void update();

	TFlipFlopComp(ifstream* fs, bool* dataPoints);
};


class ButtonComp : public cComponent{
public:
	char type = BUTTON;
	void update();

	ButtonComp(ifstream* fs, bool* dataPoints);
};


class AnchorComp : public cComponent{
public:
	char type = ANCHOR;
	void update();

	AnchorComp(ifstream* fs, bool* dataPoints);
};


class PortalComp : public cComponent{
public:
	char type = PORTAL;
	void update();

	PortalComp(ifstream* fs, bool* dataPoints);
};


class ExtenderComp : public cComponent{
public:
	char type = EXTENDER;
	void update();

	ExtenderComp(ifstream* fs, bool* dataPoints);
};


class CounterComp : public cComponent{
public:
	char type = COUNTER;
	void update();

	CounterComp(ifstream* fs, bool* dataPoints);
};