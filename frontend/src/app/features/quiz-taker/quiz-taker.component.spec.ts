import { ComponentFixture, TestBed } from '@angular/core/testing';

import { QuizTakerComponent } from './quiz-taker.component';

describe('QuizTakerComponent', () => {
  let component: QuizTakerComponent;
  let fixture: ComponentFixture<QuizTakerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [QuizTakerComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(QuizTakerComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
