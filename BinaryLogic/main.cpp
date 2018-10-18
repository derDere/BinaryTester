#include <iostream>
#include <fstream>
#include <stdlib.h>
#include "components.h"

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

void readFile(char* path);

sConnection* conStart;
sConnection* conEnd;

cComponent* compStart;
cComponent* compEnd;

bool* dataPoints;

int main(int argc, char** argv) {

	char* path = "C:\\Users\\deremer\\Documents\\TempWorking\\bt\\UnnamedTest.bil";

	readFile(path);

	cin.ignore();

	sConnection* con = conStart;
	while (con != NULL) {
		cout << con->from << "->" << con->to << endl;
		cout << (bool)*con->from << "->" << (bool)*con->to << endl;
		con = con->next;
	}

	cin.ignore();

	cComponent* com = compStart;
	while (com != NULL) {
		com->update();
		for (int i = 0; i < com->inputCount; i++) {
			cout << " i:" << *(com->input + i) << " aka " << (bool)**(com->input + i) << endl;
		}
		for (int i = 0; i < com->outputCount; i++) {
			cout << " o:" << *(com->output + i) << " aka " << (bool)**(com->output + i) << endl;
		}
		com = com->next;
	}

	cin.ignore();

	return 0;
}

void readComp(ifstream* fs, cComponent* comp) {
	char c, c1, c2;
	unsigned short s;

	while (fs->read(&c, 1) && c == NEXT_PARAM) {
		fs->read(&c1, 1);
		fs->read(&c2, 1);
		s = (c2 << 8) | c1;

		cout << "   i:" << s << endl;

		comp->inputCount += 1;

		comp->input = (bool**)realloc(comp->input, comp->inputCount * sizeof(bool*));

		*(comp->input + (comp->inputCount - 1)) = dataPoints + s;
	}

	while (fs->read(&c, 1) && c == NEXT_PARAM) {
		fs->read(&c1, 1);
		fs->read(&c2, 1);
		s = (c2 << 8) | c1;

		cout << "   o:" << s << endl;

		comp->outputCount += 1;

		comp->output = (bool**)realloc(comp->output, comp->outputCount * sizeof(bool*));

		*(comp->output + (comp->outputCount - 1)) = dataPoints + s;
	}
};

void setNextCon(sConnection* next) {
	if (conEnd == NULL) {
		conStart = next;
		conEnd = next;
	}
	else {
		conEnd->next = next;
		conEnd = next;
	}
}

void setNextComp(cComponent* next) {
	if (compEnd == NULL) {
		compStart = next;
		compEnd = next;
	}
	else {
		compEnd->next = next;
		compEnd = next;
	}
}

void readFile(char* path) {
	ifstream fs(path, ios::binary | ios::in);

	if (fs.is_open()) {
		char c, c1, c2;
		unsigned short s, from, to;
		sConnection* newCon;
		cComponent* newComp;

		while (fs.read(&c, 1)) {
			switch (c)
			{
			case DATA_POINT:
				fs.read(&c1, 1);
				fs.read(&c2, 1);
				s = (c2 << 8) | c1;
				cout << "DPs:" << s << endl;
				dataPoints = (bool*)calloc(s, sizeof(bool));
				break;

			case CONNECTION:
				fs.read(&c1, 1);
				fs.read(&c2, 1);
				from = (c2 << 8) | c1;
				fs.read(&c, 1); // NEXT_PARAM
				fs.read(&c1, 1);
				fs.read(&c2, 1);
				to = (c2 << 8) | c1;
				cout << "Con:" << from << "->" << to << endl;
				newCon = new sConnection();
				newCon->from = dataPoints + from;
				newCon->to = dataPoints + to;
				*newCon->to = true;
				setNextCon(newCon);
				break;

			case LEAVER:
				newComp = new LeaverComp();
				cout << "LEAVER" << endl;
				readComp(&fs, newComp);
				setNextComp(newComp);
				break;

			case NEGATE:
				newComp = new NegateComp();
				cout << "NEGATE" << endl;
				readComp(&fs, newComp);
				setNextComp(newComp);
				break;

			case AND:
				newComp = new AndComp();
				cout << "AND" << endl;
				readComp(&fs, newComp);
				setNextComp(newComp);
				break;

			case LAMP:
				newComp = new LampComp();
				cout << "LAMP" << endl;
				readComp(&fs, newComp);
				setNextComp(newComp);
				break;

			case XOR:
				newComp = new XorComp();
				cout << "XOR" << endl;
				readComp(&fs, newComp);
				setNextComp(newComp);
				break;

			case REPEATER:
				newComp = new RepeaterComp();
				cout << "REPEATER" << endl;
				readComp(&fs, newComp);
				setNextComp(newComp);
				break;

			case TICKER:
				newComp = new TickerComp();
				cout << "TICKER" << endl;
				readComp(&fs, newComp);
				setNextComp(newComp);
				break;

			case FLIP_FLOP:
				newComp = new FlipFlopComp();
				cout << "FLIP_FLOP" << endl;
				readComp(&fs, newComp);
				setNextComp(newComp);
				break;

			case T_FLIP_FLOP:
				newComp = new TFlipFlopComp();
				cout << "T_FLIP_FLOP" << endl;
				readComp(&fs, newComp);
				setNextComp(newComp);
				break;

			case BUTTON:
				newComp = new ButtonComp();
				cout << "BUTTON" << endl;
				readComp(&fs, newComp);
				setNextComp(newComp);
				break;

			case ANCHOR:
				newComp = new AnchorComp();
				cout << "ANCHOR" << endl;
				readComp(&fs, newComp);
				setNextComp(newComp);
				break;

			case PORTAL:
				newComp = new PortalComp();
				cout << "PORTAL" << endl;
				readComp(&fs, newComp);
				setNextComp(newComp);
				break;

			case EXTENDER:
				newComp = new ExtenderComp();
				cout << "EXTENDER" << endl;
				readComp(&fs, newComp);
				setNextComp(newComp);
				break;

			case COUNTER:
				newComp = new CounterComp();
				cout << "COUNTER" << endl;
				readComp(&fs, newComp);
				setNextComp(newComp);
				break;

			case APP_END:
				cout << "END" << endl;
				break;

			case NEXT_COMMAND:
				cout << "NEXT" << endl;
				break;

			default:
				cout << "..." << s << endl;
				break;
			}
		}
	}
}