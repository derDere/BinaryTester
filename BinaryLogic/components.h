struct sConnection
{
	bool* from;
	bool* to;
	sConnection* next;
};

class cComponent{
public:
	bool** input;
	int inputCount = 0;
	bool** output;
	int outputCount = 0;
	cComponent* next;

	virtual void update();
};

class LeaverComp : public cComponent{
	void update();
};

class NegateComp : public cComponent{
	void update();
};

class AndComp : public cComponent{
	void update();
};

class LampComp : public cComponent{
	void update();
};

class XorComp : public cComponent{
	void update();
};

class RepeaterComp : public cComponent{
	void update();
};

class TickerComp : public cComponent{
	void update();
};

class FlipFlopComp : public cComponent{
	void update();
};

class TFlipFlopComp : public cComponent{
	void update();
};

class ButtonComp : public cComponent{
	void update();
};

class AnchorComp : public cComponent{
	void update();
};

class PortalComp : public cComponent{
	void update();
};

class ExtenderComp : public cComponent{
	void update();
};

class CounterComp : public cComponent{
	void update();
};