export enum QuestionType {
    SingleChoice = "SingleChoice",
    MultipleChoice = "MultipleChoice"
}


export interface CreateQuizRequest {
    title: string;
}

export interface CreateQuizResponse {
    id: string;
    title: string;
}

export interface UpdateQuizRequest {
    title: string;
}


export interface CreateAnswerRequest {
    text: string;
    isCorrect: boolean;
}

export interface CreateQuestionRequest {
    type: QuestionType;
    text: string;
    answers: CreateAnswerRequest[];
}

export interface CreateAnswerResponse {
    id: string;
    text: string;
    isCorrect: boolean;
}

export interface CreateQuestionResponse {
    id: string;
    text: string;
    type: QuestionType;
    answers: CreateAnswerResponse[];
}

export interface UpdateAnswerRequest {
    text: string,
    isCorrect: boolean;
}

export interface UpdateQuestionRequest {
    type: QuestionType;
    text: string;
    answers: UpdateAnswerRequest[]
}

