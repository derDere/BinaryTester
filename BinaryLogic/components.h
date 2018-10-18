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
	unsigned short from;
	unsigned short to;
	sConnection* next;
};

class cComponent{
public:
	bool** input;
	int inputCount = 0;
	bool** output;
	int outputCount = 0;
	cComponent* next;
	char type = '\0';

	virtual void init();
	virtual void update();
};

class LeaverComp : public cComponent{
public:
	bool state = false;
	char type = LEAVER;
	void update();
};

class NegateComp : public cComponent{
public:
	char type = NEGATE;
	void init();
	void update();
};

class AndComp : public cComponent{
public:
	char type = AND;
	void update();
};

class LampComp : public cComponent{
public:
	bool state = false;
	char type = LAMP;
	void update();
};

class XorComp : public cComponent{
public:
	char type = XOR;
	void update();
};

class RepeaterComp : public cComponent{
public:
	char type = REPEATER;
	void update();
};

class TickerComp : public cComponent{
public:
	char type = TICKER;
	void update();
};

class FlipFlopComp : public cComponent{
public:
	char type = FLIP_FLOP;
	void update();
};

class TFlipFlopComp : public cComponent{
public:
	char type = T_FLIP_FLOP;
	void update();
};

class ButtonComp : public cComponent{
public:
	char type = BUTTON;
	void update();
};

class AnchorComp : public cComponent{
public:
	char type = ANCHOR;
	void update();
};

class PortalComp : public cComponent{
public:
	char type = PORTAL;
	void update();
};

class ExtenderComp : public cComponent{
public:
	char type = EXTENDER;
	void update();
};

class CounterComp : public cComponent{
public:
	char type = COUNTER;
	void update();
};