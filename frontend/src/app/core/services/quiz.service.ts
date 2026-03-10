import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { QuestionListResponse, QuizListResponse, SubmitAnswersRequest, SubmitAnswersResponse } from '../../shared/models/quiz-player.model';

@Injectable({
  providedIn: 'root',
})
export class QuizService {
  private http = inject(HttpClient);
  private apiUrl = "http://localhost:5100/api/quizzes";

  getQuizzes(): Observable<QuizListResponse[]> {
    return this.http.get<QuizListResponse[]>(this.apiUrl);
  }

  getQuestions(quizId: string): Observable<QuestionListResponse[]> {
    return this.http.get<QuestionListResponse[]>(`${this.apiUrl}/${quizId}/questions`);
  }

  submitAnswers(quizId: string, request: SubmitAnswersRequest): Observable<SubmitAnswersResponse> {
    return this.http.post<SubmitAnswersResponse>(`${this.apiUrl}/${quizId}/submit`, request);
  }
}
