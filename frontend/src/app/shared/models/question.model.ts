import { QuestionType } from "../enums/question-type.enum";

export interface Question {
    id: string;
    text: string;
    type: QuestionType;
    options: string[];
    correctAnswers: string[];
}
